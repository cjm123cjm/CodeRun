<template>
  <div class="cover" :style="{ width: width + 'px', height: height ? height + 'px' : 'auto' }">
    <img :src="proxy.globalInfo.imageUrl + cover" v-if="cover" style="object-fit: contain" />
    <div class="bg-color" :style="{ background: bgColor }" v-if="bgColor">
      {{ title }}
    </div>
  </div>
</template>

<script setup>
import { getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const props = defineProps({
  cover: {
    type: String,
  },
  width: {
    type: Number,
    default: 100,
  },
  height: {
    type: Number,
    default: 100,
  },
  bgColor: {
    type: String,
  },
  title: {
    type: String,
  },
})
</script>

<style lang="scss" scoped>
.cover {
  background-color: #ddd;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  img {
    width: 100%;
  }
  .bg-color {
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    color: #fff;
  }
}
</style>
