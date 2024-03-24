import request from '@/utils/request'

const userApi = {
  Login: '/account/login',
  Logout: '/account/logout',
  Register: '/account/register',
  // get my info
  UserInfo: '/account/getUserAllInfo'
}

/**
 * login func
 * @param parameter
 * @returns {*}
 */
export function login (parameter) {
  return request({
    url: userApi.Login,
    method: 'post',
    data: parameter
  })
}

// 注册方法
export function register (data) {
  return request({
    url: userApi.Register,
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}

export function getInfo () {
  return request({
    url: userApi.UserInfo,
    method: 'get',
    headers: {
      'Content-Type': 'application/json;charset=UTF-8'
    }
  })
}

export function logout () {
  return request({
    url: userApi.Logout,
    method: 'post',
    headers: {
      'Content-Type': 'application/json;charset=UTF-8'
    }
  })
}

// 获取验证码
export function getCodeImg () {
  return request({
    url: '/account/captchaImage',
    method: 'get'
  })
}
