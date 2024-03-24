import request from '@/utils/request'

/**
 * 新增/更新
 */
export function update(data) {
  if (data.id) {
    return request({
      url: '/Shelves/Update',
      method: 'put',
      data: data
    })
  } else {
    return request({
      url: '/Shelves/Add',
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
    url: '/Shelves/PageList',
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
    url: '/Shelves/Delete',
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
    url: '/Shelves/GetDetailById',
    method: 'get',
    params: query
  })
}

/**
 * 初始化映射点位
 * @param {*} query
 * @returns
 */
export function initPointMap(query) {
  return request({
    url: '/Shelves/InitPointMap',
    method: 'get',
    params: query
  })
}

/**
 * 根据货架获取所有点位信息
 * @param {*} query
 * @returns
 */
export function shelevsPositionList(query) {
  return request({
    url: '/Shelves/ShelevsPositionList',
    method: 'get',
    params: query
  })
}

/**
 * 手工操作-根据设备获取关联同样设备的货架
 * @param {*} query
 * @returns
 */
export function selctListForManual(query) {
  return request({
    url: '/Shelves/SelctListForManual',
    method: 'get',
    params: query
  })
}

/**
 * 更新货位信息
 * @param {*} data
 * @returns
 */
export function updateShelvesPosition(data) {
  return request({
    url: '/Shelves/UpdateShelvesPosition',
    method: 'post',
    data: data
  })
}
