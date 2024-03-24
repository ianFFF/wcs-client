import DDJ from './ddj'
import CSD from './csd'
import HJ from './hj'
import { equipmentEnum } from './config'
// import Button from './button'
export default class NodeObject {
  draw(queipmentData, layer) {
    this.nodeObjectList = []
    // // 根据不同类型渲染
    for (const item of queipmentData) {
      switch (item.type) {
        case equipmentEnum.CSD:
          var csdNode = new CSD(item)
          layer.append(csdNode.node)
          this.nodeObjectList.push(csdNode)
          break
        case equipmentEnum.HJ:
          var hjNode = new HJ(item)
          layer.append(hjNode.node)
          this.nodeObjectList.push(hjNode)
          break
        case equipmentEnum.DDJ:
          var ddjNode = new DDJ(item)
          layer.append(ddjNode.node)
          this.nodeObjectList.push(ddjNode)
          // setTimeout(() => {
          //   ddjNode.updateOption({ code: 'ddj-1', name: '堆垛机1', type: 'ddj', x: 10, y: 5, status: 'error' })
          // }, 2000)
          break
      }
    }
  }
  updateData(data) {
    // TODO 先使用简易方式更新数据,后续再做优化
    // 找到对象
    data.forEach((item) => {
      const nodeObject = this.nodeObjectList.find((node) => {
        return node.option.id === item.id
      })
      if (nodeObject) {
        // 调用对象updateData方法
        nodeObject.updateOption(item)
      }
    })
  }
}
