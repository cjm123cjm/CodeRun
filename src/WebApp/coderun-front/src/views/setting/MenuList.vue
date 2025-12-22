<template>
  <div class="menu-tree">
    <el-row :gutter="10">
      <el-col :span="7">
        <el-card class="box-card">
          <template #header>
            <div class="card-header">
              <span>菜单管理</span>
            </div>
          </template>
          <el-tree
            ref="treeRef"
            :data="treeData"
            default-expand-all
            node-key="menuId"
            :expand-on-click-node="false"
            :props="treeProps"
            highlight-current
            @node-click="nodeClick"
            class="tree-panel"
          >
            <template #default="{ node, data }" v-has="proxy.Permission.menu.edit">
              <div class="custom-node-style">
                <span class="node-title">{{ data.menuName }}</span>
                <el-dropdown trigger="click">
                  <span class="iconfont icon-More"></span>
                  <template #dropdown>
                    <el-dropdown-item @click="showEditDialog('add', data)"
                      >添加子菜单</el-dropdown-item
                    >
                    <el-dropdown-item
                      v-if="data.parentId != -1"
                      @click="showEditDialog('edit', data)"
                      >修改</el-dropdown-item
                    >
                    <el-dropdown-item v-if="data.parentId != -1" @click="delMenu(data)"
                      >删除</el-dropdown-item
                    >
                  </template>
                </el-dropdown>
              </div>
            </template>
          </el-tree>
        </el-card>
      </el-col>

      <el-col :span="17">
        <el-card class="box-card">
          <template #header>
            <div class="card-header">
              <span>菜单详情</span>
            </div>
          </template>
          <el-form :model="detailData" label-width="100px" class="detail-form">
            <el-form-item label="菜单ID:">
              {{ detailData.menuId }}
            </el-form-item>
            <el-form-item label="菜单名称:">
              {{ detailData.menuName }}
            </el-form-item>
            <el-form-item label="菜单层级:">
              <el-breadcrumb separator-class="el-icon-arrow-right">
                <el-breadcrumb-item v-for="(item, index) in detailData.menuNames" :key="index">
                  {{ item }}
                </el-breadcrumb-item>
              </el-breadcrumb>
            </el-form-item>
            <el-form-item label="菜单类型:">
              {{ detailData.menuType == 0 ? '菜单' : '按钮' }}
            </el-form-item>
            <el-form-item label="请求路径:">
              {{ detailData.menuUrl ? detailData.menuUrl : '-' }}
            </el-form-item>
            <el-form-item label="权限编码:">
              {{ detailData.permissionCode }}
            </el-form-item>
            <el-form-item label="菜单图标:">
              <span :class="['iconfont', 'icon-' + detailData.icon]" v-if="detailData.icon"></span>
              <span else>-</span>
            </el-form-item>
            <el-form-item label="排序号:">
              {{ detailData.sort }}
            </el-form-item>
          </el-form>
        </el-card>
      </el-col>
    </el-row>

    <MenuEdit ref="menuEditRef" :treeData="treeData" @reload="loadTreeData"></MenuEdit>
  </div>
</template>

<script setup>
import MenuEdit from './MenuEdit.vue'
import { ref, getCurrentInstance, nextTick } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  loadMenu: '/Menu/LoadMenuTree',
  delMenu: '/Menu/DeletedMenu',
}

const treeRef = ref()
const treeData = ref([])
const treeProps = {
  class: 'custom-tree-node',
  children: 'childMenu',
  label: 'menuName',
  value: 'menuId',
}

//加载菜单树结构
const loadTreeData = async () => {
  let result = await proxy.Request({
    url: api.loadMenu,
    dataType: 'json',
    method: 'get',
  })

  if (!result) return

  treeData.value = result.result

  nextTick(() => {
    let firstNode = result.result[0].childMenu ? result.result[0].childMenu[0] : result.result[0]
    let currentKey = firstNode.menuId
    treeRef.value.setCurrentKey(currentKey)
    const currentNode = treeRef.value.getNode(currentKey)
    nodeClick(currentNode.data, currentNode)
  })
}

//node点击
const detailData = ref({})
const nodeClick = (data, node) => {
  let menuNames = []
  getMenuNames(node, menuNames)
  data.menuNames = menuNames
  Object.assign(detailData.value, data)
}
const getMenuNames = (node, menuNames) => {
  if (node.data.menuName) {
    menuNames.unshift(node.data.menuName)
  }
  if (node.parent) {
    getMenuNames(node.parent, menuNames)
  }
}

//删除菜单
const delMenu = (data) => {
  proxy.Confirm('是否确认删除该菜单？', async () => {
    debugger
    let result = await proxy.Request({
      url: api.delMenu,
      dataType: 'json',
      method: 'post',
      params: data.menuId,
    })
    if (!result) return

    proxy.Message.success('删除成功')
    loadTreeData()
  })
}

//添加子菜单
const menuEditRef = ref()
const showEditDialog = (type, data) => {
  menuEditRef.value.show(type, data)
}

loadTreeData()
</script>

<style lang="scss" scoped>
.menu-tree {
  .card-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    .iconfont {
      color: #409eff;
      font-style: 20px;
      cursor: pointer;
    }
  }
  .tree-panel {
    overflow: auto;
    height: calc(100vh - 220px);
  }
  :deep .el-tree-node__content {
    height: 40px;
  }

  .custom-node-style {
    flex: 1;
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: 14px;
  }
}
</style>
