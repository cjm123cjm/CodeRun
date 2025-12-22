const regs = {
  email: /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/,
  number: /^([0]|[1-9][0-9]*)$/,
  password: /^(?=.*\d)(?=.*[a-zA-Z])[\da-zA-Z~@#$%^&*_]{8,}$/,
  shareCode: /^[A-Za-z0-9]+$/,
  version: /^[0-9\.]+$/,
  phone:
    /^[1](([3][0-9])|([4][0,1,4-9])|([5][0-3,5-9])|([6][2,5,6,7])|([7][0-8])|([8][0-9])|([9][0-3,5-9]))[0-9]{8}$/,
}

const verify = (rule, value, reg, callback) => {
  if (value) {
    if (reg.test(value)) {
      callback()
    } else {
      callback(new Error(rule.message))
    }
  } else {
    callback()
  }
}

export default {
  email: (rule, value, callback) => {
    return verify(rule, value, regs.email, callback)
  },
  number: (rule, value, callback) => {
    return verify(rule, value, regs.number, callback)
  },
  password: (rule, value, callback) => {
    return verify(rule, value, regs.password, callback)
  },
  shareCode: (rule, value, callback) => {
    return verify(rule, value, regs.shareCode, callback)
  },
  phone: (rule, value, callback) => {
    return verify(rule, value, regs.phone, callback)
  },
}
