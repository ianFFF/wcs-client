import request from '@/utils/request'

/**
 * 获取所有设备列表
 * @returns
 */
export function getAllEquipment(query) {
  return request({
    url: '/Equipment/GetAllEquipments',
    method: 'get',
    params: query
  })
}

/**
 * 手工操作获取所有设备
 */
export function getAllEquipmentsManual(query) {
  return request({
    url: '/Equipment/GetAllEquipmentsManual',
    method: 'get',
    params: query
  })
}

/**
 * 新增/更新
 */
export function update(data) {
  if (data.id) {
    return request({
      url: '/Equipment/Update',
      method: 'put',
      data: data
    })
  } else {
    return request({
      url: '/Equipment/Add',
      method: 'post',
      data: data
    })
  }
}

/**
 * 获取下拉选项
 */
export function getOptions() {
  return request({
    url: '/Equipment/GetOptions',
    method: 'get'
  })
}

/**
 * 获取分页列表
 * @param {*} query
 * @returns
 */
export function getPageList(query) {
  return request({
    url: '/Equipment/PageList',
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
    url: '/Equipment/Delete',
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
    url: '/Equipment/GetDetailById',
    method: 'get',
    params: query
  })
}
