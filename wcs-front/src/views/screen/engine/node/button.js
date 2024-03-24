import { Label } from 'spritejs'
export default class Button extends Label {
  connect(parent, zOrder) {
    super.connect(parent, zOrder)

    this.addEventListener('mouseenter', (evt) => {
      this.attr({
        fillColor: '#fee139',
        bgcolor: '#32ab61'
      })
    })

    this.addEventListener('mousedown', (evt) => {
      this.attr({
        scale: 0.95
      })
    })

    this.addEventListener('mouseup', (evt) => {
      this.attr({
        scale: 1
      })
    })

    this.addEventListener('mouseleave', (evt) => {
      this.attr({
        fillColor: '#32ab61',
        bgcolor: ''
      })
    })
  }
}
