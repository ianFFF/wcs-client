import { Group, SpriteSvg } from 'spritejs'
import { defaultOption } from './config'
const statusNormalSvg = require('../assets/circle-normal.svg')
const statusErrorSvg = require('../assets/circle-error.svg')
const csdSSvg = require('../assets/csd-s.svg')
const csdXSvg = require('../assets/csd-x.svg')
const csdZSvg = require('../assets/csd-z.svg')
const csdYSvg = require('../assets/csd-y.svg')

export default class CSD {
  constructor(option) {
    this.option = option
    this.init()
  }
  init() {
    const { option } = this
    const group = new Group({
      size: defaultOption.size,
      pos: [option.point.layout_x * defaultOption.size[0], option.point.layout_y * defaultOption.size[1]]
    })
    this.node = group

    let svgPath = null

    switch (option.direction) {
      case 90:
        svgPath = csdSSvg
        break
      case 180:
        svgPath = csdXSvg
        break
      case 240:
        svgPath = csdZSvg
        break
      default:
        svgPath = csdYSvg
        break
    }
    // 主元素
    var svg = new SpriteSvg({
      svgText: svgPath,
      size: defaultOption.size
    })
    group.append(svg)

    // 状态元素
    this.updateStatus(option.status)
  }
  updateOption(newOption) {
    const { option } = this
    const needChecks = ['status']
    for (const item of needChecks) {
      if (newOption[item] !== option[item]) {
        this.updateStatus(newOption[item])
        option[item] = newOption[item]
      }
    }
  }
  updateStatus(status) {
    var statusNode = new SpriteSvg({
      svgText: status === 1 ? statusErrorSvg : statusNormalSvg,
      size: defaultOption.statusSize
    })
    this.statusNode && this.statusNode.remove()
    this.node.append(statusNode)
    this.statusNode = statusNode
  }
}
