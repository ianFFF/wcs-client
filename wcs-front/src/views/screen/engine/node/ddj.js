import { Group, SpriteSvg } from 'spritejs'
import { defaultOption } from './config'
const bgSvg = require('../assets/ddj.svg')
const statusNormalSvg = require('../assets/circle-normal.svg')
const statusErrorSvg = require('../assets/circle-error.svg')

export default class DDJ {
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
    group.extTarget = this
    this.node = group

    // 主元素
    var svg = new SpriteSvg({
      svgText: bgSvg,
      size: defaultOption.size
    })
    group.append(svg)

    // 状态元素
    this.updateStatus(option.status)
  }
  updateOption(newOption) {
    const { option } = this
    const needChecks = ['point', 'status', 'direction']
    for (const field of needChecks) {
      if (newOption[field] !== option[field]) {
        if (field === 'status') {
          this.updateStatus(newOption[field])
          option[field] = newOption[field]
        } else if (field === 'point') {
          this.node.animate(
            [
              { x: this.option.point.layout_x * defaultOption.size[0] },
              { x: newOption.point.layout_x * defaultOption.size[0] }
            ],
            {
              duration: defaultOption.duration,
              direction: 'alternate',
              fill: 'forwards'
            }
          )
          this.option.point.layout_x = newOption.point.layout_x
        }
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
