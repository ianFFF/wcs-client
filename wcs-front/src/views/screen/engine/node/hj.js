import { Group, SpriteSvg, Label } from 'spritejs'
import { defaultOption } from './config'
const bgSvg = require('../assets/hj.svg')
const bg1Svg = require('../assets/hj-1.svg')
const bg2Svg = require('../assets/hj-2.svg')
const bg3Svg = require('../assets/hj-3.svg')
const statusNormalSvg = require('../assets/circle-normal.svg')
const statusErrorSvg = require('../assets/circle-error.svg')
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
    group.extTarget = this
    this.node = group

    // 主元素
    this.updateLoadedStatus(option.cur_loaded)

    const nameLabel = new Label(option.name)
    nameLabel.attr({
      anchor: [0, 0],
      pos: [4, 4],
      font: `bold ${defaultOption.textSize} Arial`,
      fillColor: '#676767',
      zIndex: 3
    })
    group.append(nameLabel)

    group.addEventListener('click', (evt) => {
      group.animate([{ opacity: 1 }, { opacity: 0.2 }, { opacity: 1 }], {
        duration: 500,
        fill: 'forwards',
        easing: 'ease-out'
      })
      window.EventBus.$emit('onClickHj', this.option)
    })
    // // 状态元素
    // this.updateStatus(option.status)
  }
  updateOption(newOption) {
    const { option } = this
    const needChecks = ['cur_loaded']
    for (const item of needChecks) {
      if (newOption[item] !== option[item]) {
        if (item === 'cur_loaded') {
          this.updateLoadedStatus(newOption[item])
        }
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
  updateLoadedStatus(loadedStatus) {
    let temp = bgSvg
    switch (loadedStatus) {
      case 1:
        temp = bg1Svg
        break
      case 2:
        temp = bg2Svg
        break
      case 3:
        temp = bg3Svg
        break
    }
    var svgNode = new SpriteSvg({
      svgText: temp,
      size: defaultOption.size
    })
    this.svgNode && this.svgNode.remove()
    this.node.append(svgNode)
    this.svgNode = svgNode
  }
}
