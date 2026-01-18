<template>
  <Navbar :showLeft="false" title="码上刷题"></Navbar>
  <view class="content">
    <view class="uni-margin-wrap">
      <swiper class="swiper-box" autoplay="true" indicator-dots>
        <swiper-item v-for="(item, index) in carouselList" :key="index">
          <view class="swiper-item">
            <image
              @click="swiperJump(item)"
              :src="proxy.Api.domain + proxy.Api.imagePerview + item.imgPath"
              mode="aspectFit"
              :style="{ width: '100%' }"
            ></image>
          </view>
        </swiper-item>
      </swiper>
    </view>
  </view>
  <view class="quick-list">
    <view class="quick-item search" @click="goSearch">
      <view class="title">搜索入口</view>
      <view class="summary">支持通过关键词搜索</view>
      <view class="btn">去搜索</view>
    </view>
    <view class="quick-item interview" @click="goExam">
      <view class="title">在线考试</view>
      <view class="summary">考考你学的怎么样</view>
      <view class="btn">去考试</view>
    </view>
  </view>
  <view class="category-title">八股文分类</view>
  <view class="category-list">
    <uni-grid :column="3" :showBorder="false" :square="false">
      <uni-grid-item
        v-for="(value, index) in questionCategoryInfo.getInfo()"
        :key="index"
        :index="index"
      >
        <view class="category-item">
          <view
            class="category-item-inner"
            :style="{ background: value.bgColor }"
            @click="goQuestion(item)"
          >
            <image
              v-if="value.iconPath"
              :src="proxy.Api.domain + proxy.Api.imagePerview + value.iconPath"
              mode="aspectFit"
              :style="{ width: '100%' }"
            ></image>
            <view v-else>{{ value.categoryName }}</view>
          </view>
        </view>
      </uni-grid-item>
    </uni-grid>
  </view>
</template>

<script setup>
import { onLoad } from "@dcloudio/uni-app";
import { ref, getCurrentInstance } from "vue";
import { useQuestionCategoryStore } from "@/stores/questionCategory";

const { proxy } = getCurrentInstance();
const questionCategoryInfo = useQuestionCategoryStore();

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

//搜索
const goSearch = () => {
  uni.navigateTo({
    url: "/pages/search/SearchIndex",
  });
};

//去考试
const goExam = () => {
  uni.switchTab({
    url: "./exam/Exam",
  });
};

//八股文详情
const goQuestion = (data) => {
  uni.navigateTo({
    url: `/pages/question/QuestionList?categoyId=${
      data.categoryId
    }&categoryName=${encodeURIComponent(data.categoryName)}`,
  });
};

//轮播图跳转
const swiperJump = (data) => {
  let url = `/pages/share/ShareDetail?shareId=${data.objectId}`;
  //分享
  if (data.objectType == 0) {
    url = `/pages/share/ShareDetail?shareId=${data.objectId}`;
  }
  //问题
  else if (data.objectType == 1) {
    url = `/pages/question/QuestionDetail?shareId=${data.objectId}`;
  }
  //考题
  else if (data.objectType == 2) {
    url = `/pages/exam/ExamDetail?shareId=${data.objectId}`;
  }
  //外部连接
  else if (data.objectType == 3) {
    url = `/pages/exam/WebView?url=${encodeURI(data.outerLink)}`;
  }
  uni.navigateTo({
    url: url,
  });
};
</script>

<style lang="scss" scoped>
.top {
  padding: 0px 10px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #9005fa;
  height: 40px;

  .logo {
    color: #fff;
    font-size: 20px;
  }

  .search {
    font-size: 25px;
    color: #fff;
  }
}

.uni-margin-wrap {
  width: 100%;
  background-color: #e0e0e0;
  .swiper-box {
    height: 200px;
  }
  .swiper-item {
    justify-content: center;
    align-items: center;
    display: flex;
    height: 200px;
  }
}

.quick-list {
  display: flex;
  margin-top: 20px;

  .quick-item {
    width: 100%;
    margin: 0px 5px;
    border-radius: 5px;
    padding: 15px;
    color: #fff;

    .title {
      font-weight: bold;
    }

    .summary {
      font-size: 14px;
      margin-top: 2px;
    }

    .btn {
      margin-top: 10px;
      float: left;
      font-size: 12px;
      border-radius: 5px;
      padding: 4px 10px 4px 10px;
    }
  }

  .search {
    background-color: #6663cd;

    .btn {
      background-color: #8891ed;
    }
  }

  .interview {
    background-color: #aa6bd9;
    .btn {
      background-color: #874ec1;
    }
  }
}

.category-title {
  margin-top: 20px;
  padding-left: 10px;
  font-weight: bolder;
  color: #464646;
}

.category-list {
  margin-top: 10px;
  overflow: hidden;
  padding: 0px 5px 10px 5px;
  .category-item {
    padding: 3px;
    .category-item-inner {
      height: 100px;
      border-radius: 5px;
      color: #fff;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: bold;
      font-size: 20px;
      overflow: hidden;
    }
  }
}
</style>
