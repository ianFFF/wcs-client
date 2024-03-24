import { Scene } from 'spritejs'
import NodeObject from '../node'
import { defaultOption } from '../node/config.js'
import { calcCenterPos, calcMaxSize } from '../utils'
import debounce from 'lodash.debounce'
// import { useDevicePixelRatio } from '@vueuse/core'
export default class Editor {
  constructor(container, width, height) {
    this.container = container
    this.width = width
    this.height = height
    // const { pixelRatio } = useDevicePixelRatio()
    this.scene = new Scene({ container, displayRatio: 1 })
    window.onresize = debounce(this.handleResize.bind(this), 500)
    this.nodeObject = new NodeObject()
  }
  // 渲染布局
  draw(objectData) {
    this.objectData = objectData
    const layer = this.createLayer()
    // 分别渲染设备
    this.nodeObject.draw(objectData, layer)
    this.layer = layer
  }
  // 创建画布
  createLayer(name = null) {
    const layer = this.scene.layer(name)
    var maxSize = calcMaxSize(this.objectData)
    // layer.canvas.style.backgroundColor = '#D1D6DE'
    const centerPost = calcCenterPos(
      this.width,
      this.height,
      maxSize[0] * defaultOption.size[0],
      maxSize[1] * defaultOption.size[1]
    )
    layer.attributes.pos = [centerPost[0], centerPost[1]]
    return layer
  }
  handleResize(e) {
    if (this.layer) {
      const width = this.container.clientWidth
      const height = this.container.clientHeight
      var maxSize = calcMaxSize(this.objectData)

      const centerPost = calcCenterPos(
        width,
        height,
        maxSize[0] * defaultOption.size[0],
        maxSize[1] * defaultOption.size[1]
      )
      this.layer.attributes.pos = [centerPost[0], centerPost[1]]
    }
  }
  updateData(data) {
    this.nodeObject.updateData(data)
  }
}
