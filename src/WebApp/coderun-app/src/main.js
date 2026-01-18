import { createSSRApp } from "vue";
import * as Pinia from "pinia";
import App from "./App.vue";

import "@/static/base.scss";
import "@/static/icon/iconfont.css";

import Navbar from "@/pages/components/common/Navbar.vue";

import Message from "./utils/Message";
import Request from "./utils/Request";
import Api from "./utils/Api";

export function createApp() {
  const app = createSSRApp(App);

  app.use(Pinia.createPinia());

  app.component("Navbar", Navbar);

  app.config.globalProperties.Message = Message;
  app.config.globalProperties.Request = Request;
  app.config.globalProperties.Api = Api;

  return {
    app,
  };
}
