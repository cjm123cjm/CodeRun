import Message from "./Message";
import Api from "./Api";

const contentTypeForm = "application/x-www-form-urlencoded;charset=UTF-8";
const contentTypeJson = "application/json;charset=UTF-8";
const responseTypeJson = "json";

let port = uni.getSystemInfoSync().uniPlatform;
let BASE_URL = null;
if (port == "web") {
  BASE_URL = Api.domain + "/api";
} else {
  BASE_URL = Api.domain + "/api";
}

const Request = (config) => {
  const {
    url,
    method,
    params,
    dataType,
    showLoading = true,
    showError = true,
    errorCallback,
    responseType = responseTypeJson,
  } = config;
  let contentType = contentTypeForm;
  let data = null;

  // 根据 dataType 处理数据
  if (dataType === "json") {
    contentType = contentTypeJson;
    data = JSON.stringify(params || {});
  }

  let headers = {
    "Content-Type": contentType,
    "X-Requested-With": "XMLHttpRequest",
    token: "",
  };

  if (params) {
    for (let item in params) {
      if (params[item] == undefined) {
        params[item] = "";
      }
    }
  }

  return new Promise((resolve, reject) => {
    if (showLoading) {
      uni.showLoading();
    }
    uni
      .request({
        url: BASE_URL + url,
        data: params,
        header: headers,
        responseType: responseType,
        method: method,
      })
      .then((res) => {
        if (showLoading) {
          uni.hideLoading();
        }
        uni.stopPullDownRefresh();
        if (res.statusCode != 200) {
          return Promise.reject("网络连接错误");
        }
        const responseData = res.data;
        if (responseType == "arraybuffer" || responseType == "blob") {
          resolve(responseData);
          return;
        }
        if (responseData.code == 200) {
          if (responseData.isSuccess) {
            resolve(responseData);
          } else {
            Message.error(responseData.message)
          }
          return;
        } else if (responseData.code == 403) {
          return Promise.reject(responseData.message);
        } else if (responseData.code == 401) {
          uni.navigateTo({
            url: "/pages/account/LoginAndRegister",
          });
          return Promise.reject();
        } else {
          if (errorCallback) {
            errorCallback(responseData.message);
          }
          return Promise.reject(responseData.message);
        }
      })
      .catch((error) => {
        if (error && showError) {
          Message.error(error);
        }
        if (error.status == 401) {
          uni.navigateTo({
            url: "/pages/account/LoginAndRegister",
          });
          return;
        }
        return null;
      });
  });
};

export default Request;
