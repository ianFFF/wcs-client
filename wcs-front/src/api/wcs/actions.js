import request from '@/utils/request'

/**
 * 启动自动入库
 * @returns
 */
export function autoRkModeStart() {
  return request({
    url: '/Task/AutoRkModeStart',
    method: 'get'
  })
}

/**
 * 关闭自动入库
 * @returns
 */
export function autoRkModeStop() {
  return request({
    url: '/Task/AutoRkModeStop',
    method: 'put'
  })
}

/**
 * 开启自动处理任务
 * @returns
 */
export function dealTaskStart() {
  return request({
    url: '/Task/DealTaskStart',
    method: 'get'
  })
}

/**
 * 停止自动处理任务
 * @returns
 */
export function dealTaskStop() {
  return request({
    url: '/Task/DealTaskStop',
    method: 'put'
  })
}

/**
 * 获取所有设备信息
 * @returns
 */
export function allObject() {
  return request({
    url: '/Task/AllObjects',
    method: 'get'
  })
}
