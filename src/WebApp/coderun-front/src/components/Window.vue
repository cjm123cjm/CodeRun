<template>
  <teleport to="#app">
    <div class="window" v-if="show">
      <div class="window-mask" v-if="show"></div>
      <div class="close" @click="close">
        <span class="iconfont icon-guanbi"></span>
      </div>
      <div class="window-content">
        <slot></slot>
      </div>
    </div>
  </teleport>
</template>

<script setup>
const props = defineProps({
  show: {
    type: Boolean,
    default: false,
  },
})
const emits = defineEmits(['close'])
const close = () => {
  emits('close')
}
</script>

<style lang="scss" scoped>
.window {
  .window-mask {
    top: 0px;
    left: 0px;
    width: 100%;
    height: calc(100vh);
    z-index: 200;
    opacity: 0.5;
    background-color: #000;
    position: fixed;
  }
  .close {
    z-index: 202;
    cursor: pointer;
    position: absolute;
    top: 40px;
    right: 30px;
    height: 44px;
    width: 44px;
    border-radius: 22px;
    background: #606266;
    display: flex;
    justify-content: center;
    align-items: center;
    .iconfont {
      font-size: 20px;
      color: #fff;
      z-index: 10000;
    }
  }
  .window-content {
    top: 0px;
    z-index: 201;
    position: absolute;
    width: 100%;
    height: calc(100vh);
    overflow: auto;
  }
}
</style>
