<template>
  <div class="top-panel">
    <el-card>
      <el-form :model="searchForm" label-width="70px" label-position="right">
        <el-row>
          <el-col :span="5">
            <el-form-item label="角色名称">
              <el-input
                class="password-input"
                v-model="searchForm.roleName"
                clearable
                placeholder="支持模糊搜索"
                @keyup.enter.native="loadDataList"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="5">
            <el-form-item label="描述">
              <el-input
                class="password-input"
                v-model="searchForm.roleDesc"
                clearable
                placeholder="支持模糊搜索"
                @keyup.enter.native="loadDataList"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="4" :style="{ paddingLeft: '10px' }">
            <el-button type="success" @click="loadDataList">查询</el-button>
            <el-button type="primary" @click="addRole" v-has="proxy.PermissionCodes.role.edit"
              >新增角色</el-button
            >
          </el-col>
        </el-row>
      </el-form>
    </el-card>
  </div>
  <el-row :gutter="20" :style="{ 'margin-top': '10px' }" class="table-data-card">
    <el-col :span="18">
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            <span>角色列表</span>
          </div>
        </template>
        <Table
          ref="tableDataRef"
          :columns="columns"
          :fetch="loadDataList"
          :dataSource="tableData"
          :options="tableOptions"
          :extHeight="tableOptions.extHeight"
          @row-click="handleRowClick"
        >
          <template #operation="{ index, row }">
            <div class="row-op-panel">
              <a
                class="a-link"
                href="javascript:void(0)"
                @click="editRole(row)"
                v-has="proxy.PermissionCodes.role.edit"
                >修改</a
              >
              <a
                class="a-link"
                href="javascript:void(0)"
                @click="delRole(row)"
                v-has="proxy.PermissionCodes.role.del"
                >删除</a
              >
            </div>
          </template>
        </Table>
      </el-card>
    </el-col>
    <el-col :span="6">
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            <span>菜单信息</span>
            <el-button type="primary" @click="saveRoleMenu" :style="{ float: 'right' }"
              >保存</el-button
            >
          </div>
        </template>
        <div class="detail-tree-panel">
          <el-tree
            ref="menuTreeRef"
            node-key="menuId"
            show-checkbox
            :data="treeData"
            default-expand-all
            v-if="treeData.length"
            :props="replaceFields"
            :check-strictly="false"
          />
        </div>
      </el-card>
    </el-col>
  </el-row>
  <RoleEdit ref="roleEditRef" @reload="loadDataList" :treeData="treeData"></RoleEdit>
</template>

<script setup>
import { ref, getCurrentInstance } from 'vue'
import RoleEdit from './RoleEdit.vue'
const { proxy } = getCurrentInstance()
const api = {
  LoadRoleList: 'role/loadRoleList',
  loadMenu: '/Menu/LoadMenuTree',
  RoleMenuByRoleId: '/Role/RoleMenuByRoleId',
  SaveRoleMenu: '/Role/SaveRoleMenu',
  DeletedRole: '/Role/DeletedRole',
}

const searchForm = ref({})
const currentRow = ref({})
const tableData = ref({
  data: [],
})
const tableDataRef = ref()
const tableOptions = ref({
  extHeight: 125,
})

//加载角色列表
const loadDataList = async () => {
  let params = {
    roleName: searchForm.value.roleName,
    roleDesc: searchForm.value.roleDesc,
  }
  const result = await proxy.Request({
    url: api.LoadRoleList,
    method: 'get',
    dataType: 'json',
    params: params,
  })
  if (!result) return

  tableData.value = {
    data: result.result || [],
  }
  if (Object.keys(currentRow.value).length == 0 && tableData.value.data.length > 0) {
    Object.assign(currentRow.value, tableData.value.data[0])
    handleRowClick(currentRow.value)
  }
  tableDataRef.value.setCurrentRow('roleId', currentRow.value.roleId)
}

const columns = [
  {
    label: '角色名称',
    prop: 'roleName',
  },
  {
    label: '描述',
    prop: 'roleDesc',
  },
  {
    label: '操作',
    width: '150',
    scopedSlots: 'operation',
  },
]

//行单击
const handleRowClick = async (row) => {
  Object.assign(currentRow.value, row)
  //查询用户权限
  const result = await proxy.Request({
    url: api.RoleMenuByRoleId,
    method: 'get',
    dataType: 'json',
    params: { roleId: row.roleId },
  })
  if (!result) return
  menuTreeRef.value.setCheckedKeys(result.result.menuIds)
}

const menuTreeRef = ref()
const treeData = ref([])
const replaceFields = {
  class: 'custom-tree-node',
  children: 'childMenu',
  label: 'menuName',
  value: 'menuId',
}
const loadMenuTree = async () => {
  const result = await proxy.Request({
    url: api.loadMenu,
    method: 'get',
    dataType: 'json',
  })
  if (!result) return
  treeData.value = result.result || []
}
loadMenuTree()

//保存角色菜单权限
const saveRoleMenu = async () => {
  let menuIdArray = menuTreeRef.value.getCheckedKeys()
  let halfMenuIdArray = menuTreeRef.value.getHalfCheckedKeys() || []
  const result = await proxy.Request({
    url: api.SaveRoleMenu,
    dataType: 'json',
    method: 'post',
    params: {
      roleId: currentRow.value.roleId,
      roleName: currentRow.value.roleName,
      menuIds: menuIdArray.join(','),
      halfMenuIds: halfMenuIdArray.join(','),
    },
  })
  if (!result) return

  proxy.Message.success('保存成功')
}

//删除角色
const delRole = (role) => {
  proxy.Confirm(`确定删除【${role.roleName}】角色吗?`, async () => {
    let result = await proxy.Request({
      url: api.DeletedRole,
      method: 'post',
      dataType: 'json',
      params: role.roleId,
    })
    if (!result) return
    proxy.Message.success('删除成功')
    loadDataList()
  })
}

const roleEditRef = ref()
//添加角色
const addRole = () => {
  roleEditRef.value.open({ roleId: 0 })
}

//修改角色
const editRole = (row) => {
  roleEditRef.value.open(Object.assign({}, row))
}
</script>

<style lang="scss" scoped>
.detail-tree-panel {
  height: calc(100vh - 273px);
  overflow: auto;
  width: 100%;
}
</style>
