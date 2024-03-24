import request from '@/utils/request'

/**
 * 获取所有异常等级信息
 * @returns
 */
export function getAllAlarm() {
  return request({
    url: '/EquipmentAlarm/GetAllAlarm',
    method: 'get'
  })
}
/**
 * 新增/更新
 */
export function update(data) {
  if (data.id) {
    return request({
      url: '/EquipmentAlarm/Update',
      method: 'put',
      data: data
    })
  } else {
    return request({
      url: '/EquipmentAlarm/Add',
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
    url: '/EquipmentAlarm/PageList',
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
    url: '/EquipmentAlarm/Delete',
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
    url: '/EquipmentAlarm/GetDetailById',
    method: 'get',
    params: query
  })
}
