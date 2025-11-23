<template>
  <el-dialog
    :show-close="showClose"
    :draggable="true"
    :model-value="show"
    :close-on-click-modal="false"
    :title="title"
    class="cust-dialog"
    :top="top + 'px'"
    :width="width"
    @close="close"
  >
    <div class="dialog-body" :style="{ 'max-height': maxHeight + 'px', padding: padding + 'px' }">
      <slot></slot>
    </div>
    <template v-if="(buttons && buttons.length > 0) || showCancel">
      <div class="dialog-footer">
        <el-button link @click="close" v-if="showCancel">取消</el-button>
        <el-button v-for="item in buttons" :type="item.type || 'primary'" @click="item.click">
          {{ item.text }}
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup>
const props = defineProps({
  //标题
  title: {
    type: String,
  },
  //是否显示
  show: {
    type: Boolean,
    default: false,
  },
  //是否展示右上角的关闭按钮
  showClose: {
    type: Boolean,
    default: true,
  },
  //是否展示底部的取消按钮
  showCancel: {
    type: Boolean,
    default: true,
  },
  //距离顶部的距离
  top: {
    type: Number,
    default: 50,
  },
  //宽度
  width: {
    type: String,
    default: '30%',
  },
  //底部按钮
  buttons: {
    type: Array,
  },
  //内边距
  padding: {
    type: Number,
    default: 15,
  },
})

const maxHeight = window.innerHeight - props.top - 100

const emits = defineEmits('close')
const close = () => {
  emits('close')
}
</script>

<style lang="scss" scoped>
.cust-dialog {
  margin: 30px auto 10px !important;
  .el-dialog__body {
    padding: 0px;
  }
  .dialog-body {
    border-top: 1px solid #ddd;
    border-bottom: 1px solid #ddd;
    min-height: 80px;
    overflow: auto;
    overflow-x: hidden;
  }
  .dialog-footer {
    text-align: right;
    padding: 5px 20px;
  }
}
</style>
