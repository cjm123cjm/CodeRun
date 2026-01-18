<template>
  <Navbar :showLeft="false" title="码上刷题"></Navbar>
  <view>{{ carouselList }}</view>
</template>

<script setup>
import { onLoad } from "@dcloudio/uni-app";
import { ref, getCurrentInstance } from "vue";

const { proxy } = getCurrentInstance();

const carouselList = ref([]);
const LoadCarousel = async () => {
  let result = await proxy.Request({
    url: proxy.Api.LoadCarousel,
    showLoading: false,
    method: "get",
  });
  if (!result) return;
  carouselList.value = result.result;
};

LoadCarousel();
</script>
