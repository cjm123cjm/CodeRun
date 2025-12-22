<template>
  <Dialog
    :title="dialogConfig.title"
    :show="dialogConfig.show"
    :showClose="dialogConfig.showClose"
    :showCancel="dialogConfig.showCancel"
    :top="dialogConfig.top"
    :buttons="dialogConfig.buttons"
    @close="dialogConfig.show = false"
  >
    <el-form :model="formData" ref="formDataRef" :rules="rules" label-width="80px" @submit.prevent>
      <el-form-item label="角色名称" prop="roleName">
        <el-input
          v-model="formData.roleName"
          placeholder="请输入角色名称"
          :maxLength="20"
        ></el-input>
      </el-form-item>
      <el-form-item label="关联菜单" prop="menuIds" v-if="formData.roleId == 0">
        <div class="tree-panel">
          <el-tree
            ref="menuTreeRef"
            node-key="menuId"
            show-checkbox
            :data="treeData"
            default-expand-all
            :props="replaceFields"
            :check-strictly="false"
            @check-change="handleMenuTreeChecked"
          />
        </div>
      </el-form-item>
      <el-form-item label="描述" prop="roleDesc">
        <el-input type="textarea" :maxLength="250" v-model="formData.roleDesc"></el-input>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  SaveRole: '/Role/SaveRole',
}
const props = defineProps({
  //角色菜单树数据
  treeData: {
    type: Array,
    default: [],
  },
})
const menuTreeRef = ref()
const replaceFields = {
  class: 'custom-tree-node',
  children: 'childMenu',
  label: 'menuName',
  value: 'menuId',
}
const handleMenuTreeChecked = () => {
  formData.value.menuIds = menuTreeRef.value.getCheckedKeys()
}

const dialogConfig = ref({
  title: '编辑角色',
  show: false,
  showClose: true,
  showCancel: true,
  top: 50,
  buttons: [
    {
      type: 'primary',
      text: '保存',
      click: () => {
        submit()
      },
    },
  ],
})

const formDataRef = ref(null)
const formData = ref({})
const rules = {
  roleName: [{ required: true, message: '请输入角色名称', trigger: 'blur' }],
  menuIds: [{ required: true, message: '请选择关联菜单', trigger: ['change'] }],
}

const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    data.menuIds = []
    formDataRef.value.resetFields()
    dialogConfig.value.title = data.roleId == 0 ? '新增角色' : '编辑角色'
    formData.value = data
  })
}
const emits = defineEmits(['reload'])

const submit = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    let params = {}
    Object.assign(params, formData.value)
    //新增
    if (formData.value.roleId == 0) {
      params.menuIds = params.menuIds.join(',')
      const halfMenuIdArray = menuTreeRef.value.getHalfCheckedKeys() || []
      params.halfMenuIds = halfMenuIdArray.join(',')
    }
    //修改
    else {
      params.menuIds = null
    }
    let result = await proxy.Request({
      url: api.SaveRole,
      dataType: 'json',
      method: 'post',
      params: params,
    })
    if (!result) return
    dialogConfig.value.show = false
    proxy.Message.success('保存成功')
    emits('reload')
  })
}

defineExpose({
  open,
})
</script>

<style lang="scss" scoped>
.tree-panel {
  width: 100%;
  overflow: auto;
  max-height: calc(100vh / 2);
}
</style>
