import request from '@/utils/request'

/**
 * 获取分页列表
 * @param {*} query
 * @returns
 */
export function getPageList(query) {
  return request({
    url: '/Task/PageList',
    method: 'get',
    params: query
  })
}

/**
 * 手工执行入库任务
 * @param {*} query
 * @returns
 */
export function manualRkMode(query) {
  return request({
    url: '/Task/ManualRkMode',
    method: 'get',
    params: query
  })
}

/**
 * 手工执行出库任务
 * @param {*} query
 * @returns
 */
export function manualCkMode(query) {
  return request({
    url: '/Task/ManualCkMode',
    method: 'get',
    params: query
  })
}

/**
 * 删除
 * @param {*} data
 * @returns
 */
export function deleteData(query) {
  return request({
    url: '/Task/Delete',
    method: 'get',
    params: query
  })
}

/**
 * 初始化仓库所有数据
 * @returns
 */
export function initAllData() {
  return request({
    url: '/Task/InitAllData',
    method: 'get'
  })
}

/**
 * 任务统计信息
 * @returns
 */
export function taskStatistics() {
  return request({
    url: '/Task/TaskStatistics',
    method: 'get'
  })
}

/**
 * 获取执行机信息
 * @param {*} query
 * @returns
 */
export function getServerMessages(query) {
  return request({
    url: '/Task/GetServerMessages',
    method: 'get',
    params: query
  })
}

/**
 * 获取最新的任务执行信息
 * @param {*} query
 * @returns
 */
export function getTop3Task(query) {
  return request({
    url: '/Task/GetTop3Task',
    method: 'get',
    params: query
  })
}

/**
 * 查询可运行设备状态
 * @returns
 */
export function getEquipmentStatus() {
  return request({
    url: '/Task/GetEquipmentStatus',
    method: 'get'
  })
}

/**
 * 更新货位信息
 * @returns
 */
export function updateConsoleShelvesPosition(query) {
  return request({
    url: '/Task/UpdateShelvesPosition',
    method: 'get',
    params: query
  })
}
