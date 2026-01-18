const Message = {
  error: (msg, callback) => {
    uni.showToast({
      title: msg,
      icon: "error",
      success: callback ? callback : null,
    });
  },
  success: (msg, callback) => {
    uni.showToast({
      title: msg,
      icon: "success",
      success: callback ? callback : null,
    });
  },
  warning: (msg, callback) => {
    uni.showToast({
      title: msg,
      icon: "none",
      success: callback ? callback : null,
    });
  },
};

export default Message
