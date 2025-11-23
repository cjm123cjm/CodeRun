<template>
  <div class="layout">
    <div class="header">
      <div class="logo">CodeRun管理后台</div>
      <div class="userinfo">
        欢迎回来,
        <el-dropdown>
          <span class="name">{{ userInfo.account.userName }}</span>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item @click="updatePassword">修改密码</el-dropdown-item>
              <el-dropdown-item @click="logout">退出</el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </div>
    </div>
    <div class="body">
      <div class="left-sider">
        <div class="menu-list">
          <div
            :class="['menu-item', currentPmenu.menuUrl == item.menuUrl ? 'active' : '']"
            v-for="item in userInfo.menus"
            @click="pMenuClickHandler(item)"
          >
            <div :class="['iconfont', 'icon-' + item.icon]"></div>
            <div class="menu-name">{{ item.menuName }}</div>
          </div>
        </div>
        <div class="menu-sub-list">
          <div
            :class="['sub-menu', currentSubMenu.menuUrl == sub.menuUrl ? 'active' : '']"
            v-for="sub in currentPmenu.childMenu"
          >
            {{ sub.menuName }}
          </div>
        </div>
      </div>
      <div class="main-content">
        <div class="tag-content">
          <el-tabs
            type="border-card"
            v-model="currentSubMenu.menuUrl"
            @tab-click="tabClick"
            @edit="tabEdit"
          >
            <el-tab-pane
              v-for="item in tabList"
              :name="item.menuUrl"
              :label="item.menuName"
              :closable="tabList.length > 1"
            ></el-tab-pane>
          </el-tabs>
        </div>
        <div class="body-content">
          <router-view></router-view>
        </div>
      </div>
    </div>

    <UpdatePassword ref="updatePasswordRef"></UpdatePassword>
  </div>
</template>

<script setup>
import UpdatePassword from './UpdatePassword.vue'
import { ref, onMounted, getCurrentInstance } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const { proxy } = getCurrentInstance()
const route = useRoute()
const router = useRouter()
const userInfo = ref(JSON.parse(sessionStorage.getItem('userInfo')) || { menus: [] })

const menuMap = ref({})
const initMenuMap = () => {
  const menuList = userInfo.value.menus
  for (let i = 0; i < menuList.length; i++) {
    const pMenu = menuList[i]
    menuMap.value[pMenu.menuUrl] = pMenu
    if (pMenu.childMenu && pMenu.childMenu.length > 0) {
      for (let j = 0; j < pMenu.childMenu.length; j++) {
        const subMenu = pMenu.childMenu[j]
        subMenu['parentMenu'] = pMenu.menuUrl
        menuMap.value[subMenu.menuUrl] = subMenu
      }
    }
  }
}
const menuSelect = (path, addTab) => {
  let curMenu = menuMap.value[path]
  if (curMenu == null) {
    return
  }
  currentSubMenu.value = curMenu
  currentPmenu.value = menuMap.value[curMenu.parentMenu]
  if (addTab) {
    addTabHandler(curMenu)
  }
}

//一级菜单
const currentPmenu = ref({})
//二级菜单
const currentSubMenu = ref({})
const pMenuClickHandler = (item) => {
  currentPmenu.value = item
  const firstMenu = item.childMenu[0]
  jump(firstMenu)
}
//跳转菜单
const jump = (data) => {
  if (currentSubMenu.value.menuUrl == data.menuUrl) {
    return
  }
  currentSubMenu.value = data
  addTabHandler(data)
  router.push(data.menuUrl)
}

//tab选项卡
const tabList = ref([])
//添加tab选项卡
const addTabHandler = (curMenu) => {
  let currentTab = tabList.value.find((t) => t.menuUrl == currentSubMenu.value.menuUrl)
  if (!currentTab) {
    tabList.value.push(curMenu)
  }
}
//tab点击事件
const tabClick = (e) => {
  const path = e.props.name
  menuSelect(path, false)
  router.push(path)
}
//去掉tab
const tabEdit = (targetKey, action) => {
  if (action !== 'remove') {
    return
  }
  let curPath = currentSubMenu.value.menuUrl
  let tabs = tabList.value
  tabs.forEach((tab, index) => {
    if (tab.menuUrl == targetKey) {
      let nextTab = tabs[index + 1] || tabs[index - 1]
      if (nextTab) {
        curPath = nextTab.menuUrl
      }
    }
  })
  tabList.value = tabs.filter((tab) => tab.menuUrl != targetKey)
  if (curPath != currentSubMenu.value.menuUrl) {
    menuSelect(curPath, false)
    router.push(curPath)
  }
}

//修改密码
const updatePasswordRef = ref()
const updatePassword = () => {
  updatePasswordRef.value.show()
}

//退出
const logout = () => {
  proxy.Confirm('确定要退出吗?', () => {
    sessionStorage.removeItem('userInfo')
    proxy.VueCookies.remove('token')
    router.push('/login')
  })
}

onMounted(() => {
  initMenuMap()
  menuSelect(route.path, true)
})
</script>

<style lang="scss" scoped>
.layout {
  .header {
    border-bottom: 1px solid #ddd;
    height: 60px;
    padding-right: 24px;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: space-between;
    .logo {
      font-weight: bold;
      margin-left: 5px;
      font-style: 25px;
      color: #05a1f5;
    }
    .userinfo {
      margin-right: 20px;
      font-style: 14px;
      display: flex;
      align-items: center;
      .name {
        font-size: 1;
        color: #409eff;
        cursor: pointer;
      }
      .user-type {
        margin-left: 5px;
      }
      .logout {
        margin-left: 15px;
      }
    }
  }

  .body {
    display: flex;
    .left-sider {
      width: 260px;
      display: flex;
      height: calc(100vh - 60px);
      border-right: 1px solid #f1f2f4;
      box-shadow: 0 3px 10px 0 rgba(14, 14, 14, 0.06);
      .menu-list {
        width: 70px;
        text-align: center;
        background: #1a1a1a;
        .menu-item {
          text-align: center;
          padding: 15px 0px;
          cursor: pointer;
          color: #fff;
          .iconfont {
            font-size: 20px;
          }
          .icon-app {
            font-weight: bold;
          }
          &:hover {
            color: #06a7ff;
          }
        }
        .active {
          background: #06a7ff;
          &:hover {
            color: #fff;
          }
        }
      }
      .menu-sub-list {
        flex: 1;
        position: relative;
        padding: 5px 5px;
        .sub-menu {
          cursor: pointer;
          padding: 10px 8px;
          border-radius: 5px;
          &:hover {
            color: #05a1f5;
          }
        }
        .active {
          background: #e8f4ff;
          color: #1890ff;
        }
      }
    }

    .main-content {
      width: 100%;
      .tag-content {
        :deep .el-tabs--border-card {
          border: none;
        }
        :deep .el-tabs__content {
          display: none;
        }
      }
      .body-content {
        padding: 10px;
      }
    }
  }
}
</style>
