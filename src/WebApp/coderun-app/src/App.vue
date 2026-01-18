<script setup>
import { onLaunch } from "@dcloudio/uni-app";
import { useAppInfoStore } from "@/stores/appInfo";
import { useQuestionCategoryStore } from "@/stores/questionCategory";
import { getCurrentInstance } from "vue";
const { proxy } = getCurrentInstance();

const appInfo = useAppInfoStore();
const questionCategoryInfo = useQuestionCategoryStore();

const saveDeviceInfo = (e) => {
  let statusBar = e.statusBarHeight;

  let navBarHeight = 45;
  const deviceId = e.deviceId;
  const deviceBrand = e.deviceBrand;

  appInfo.setInfo(
    statusBar,
    navBarHeight,
    e.screenWidth,
    e.screenHeight,
    deviceId,
    deviceBrand,
    e.appWgtVersion
  );
};

//查询分类
const loadAllCategory = async () => {
  let result = await proxy.Request({
    url: proxy.Api.LoadAllCategory,
    showLoading: false,
  });
  if (!result) return;
  questionCategoryInfo.setInfo(result.result);
};

onLaunch(() => {
  uni.getSystemInfo({
    success: (e) => {
      saveDeviceInfo(e);
    },
  });

  loadAllCategory()
});
</script>

<style lang="scss">
/*每个页面公共css */
</style>
