import request from '@/utils/request'

/**
 * 新增/更新
 */
export function update(data) {
  if (data.id) {
    return request({
      url: '/EquipmentLogs/Update',
      method: 'put',
      data: data
    })
  } else {
    return request({
      url: '/EquipmentLogs/Add',
      method: 'post',
      data: data
    })
  }
}

/**
 * 获取分页列表
 * @param {*} query
 * @returns
 */
export function getPageList(query) {
  return request({
    url: '/EquipmentLogs/PageList',
    method: 'get',
    params: query
  })
}

/**
 * 删除
 * @param {*} data
 * @returns
 */
export function deleteData(data) {
  return request({
    url: '/EquipmentLogs/Delete',
    method: 'delete',
    data: data
  })
}

/**
 * 单个查询
 * @param {*} query
 * @returns
 */
export function getDetailById(query) {
  return request({
    url: '/EquipmentLogs/GetDetailById',
    method: 'get',
    params: query
  })
}

/**
 * 获取运行日志
 * @param {*} query
 * @returns
 */
export function getRunningLogsList(query) {
  return request({
    url: '/EquipmentRecords/PageList',
    method: 'get',
    params: query
  })
}

/**
 * 获取故障日志
 * @param {*} query
 * @returns
 */
export function getErrorLogsList(query) {
  return request({
    url: '/EquipmentRecords/PageList',
    method: 'get',
    params: query
  })
}

/**
 * 获取异常类型
 * @returns
 */
export function GetAlarmType() {
  return request({
    url: '/EquipmentRecords/GetAlarmType',
    method: 'get'
  })
}

/**
 * 获取异常编码
 * @returns
 */
export function GetAlarmCode() {
  return request({
    url: '/EquipmentRecords/GetAlarmCode',
    method: 'get'
  })
}
