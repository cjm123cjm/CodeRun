<script setup>
import { onLaunch } from "@dcloudio/uni-app";
import { useAppInfoStore } from "@/stores/appInfo";

const appInfo = useAppInfoStore();

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

onLaunch(() => {
  uni.getSystemInfo({
    success: (e) => {
      saveDeviceInfo(e);
    },
  });
});
</script>

<style lang="scss">
/*每个页面公共css */
</style>
