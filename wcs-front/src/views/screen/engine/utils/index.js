/**
 * 计算物体在容器的居中位置
 * @param {容器宽度} containerW
 * @param {容器高度} containerH
 * @param {居中物体宽度} width
 * @param {居中物体高度} height
 * @returns x,y数组
 */
function calcCenterPos(containerW, containerH, width, height) {
  return [containerW / 2 - width / 2, containerH / 2 - height / 2]
}

/**
 * 计算最大Size
 * @param {*} data
 * @returns
 */
function calcMaxSize(data) {
  var maxX = 0
  var maxY = 0
  data.forEach((item) => {
    var tempX = parseInt(item.point.layout_x)
    if (maxX < tempX) {
      maxX = tempX
    }
    var tempY = parseInt(item.point.layout_y)
    if (maxY < tempY) {
      maxY = tempY
    }
  })
  return [maxX, maxY]
}
export { calcCenterPos, calcMaxSize }
