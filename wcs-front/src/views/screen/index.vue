<template>
  <transition name="fade">
    <div class="screen-container">
      <div id="stage" style="width: 100vw; height: 100vh"></div>
      <summary-box></summary-box>
      <footer-box></footer-box>
      <a-modal title="货架正视信息" width="1000px" :visible="visible" @cancel="visible = false" :footer="null">
        <front-view :shelvesId="shelvesId" v-if="visible"></front-view>
      </a-modal>
    </div>
  </transition>
</template>

<script>
import Editor from './engine/editor'
import FrontView from './frontView'
import SummaryBox from './summary.vue'
import FooterBox from './footer.vue'
import { defaultOption } from './engine/node/config.js'
import { allObject } from '@/api/wcs/actions'

// import { Group, Label, SpriteSvg } from 'spritejs'
export default {
  components: { FrontView, SummaryBox, FooterBox },
  data() {
    return {
      visible: false,
      editor: null,
      objectData: null,
      shelvesId: null,
      isFirst: true,
      intervalGetData: null
    }
  },
  created() {
    window.EventBus.$on('onClickHj', this.handleHjClick)
  },
  beforeDestroy() {
    window.EventBus.$off('onClickHj')
    if (this.intervalGetData) {
      clearInterval(this.intervalGetData)
    }
  },
  mounted() {
    const container = document.getElementById('stage')
    const editor = new Editor(container, container.clientWidth, container.clientHeight)
    this.editor = editor
    this.getData()
    // 模拟数据加载
    // setTimeout(() => {
    //   const cloneObject = clonedeep(this.objectData)
    //   const cur = cloneObject.find((item) => item.type === 'HW')
    //   cur.cur_loaded = 3
    //   const ddj = cloneObject.find((item) => item.type === 'DDJ')
    //   ddj.point.layout_x = 5
    //   ddj.status = 1
    //   const ssx = cloneObject.find((item) => item.type === 'SSX')
    //   ssx.status = 1
    //   this.editor.updateData(cloneObject)
    // }, 2000)
    this.intervalGetData = setInterval(() => {
      this.getData()
    }, defaultOption.duration)
  },
  methods: {
    handleHjClick(e) {
      this.shelvesId = e.shelves_id
      this.visible = !this.visible
    },
    async getData() {
      await allObject().then((res) => {
        this.loading = false
        if (res.status) {
          if (this.isFirst) {
            this.isFirst = false
            // 首次加载
            this.objectData = res.data.all_objects
            this.editor.draw(this.objectData)
          } else {
            // 增量更新
            this.objectData = res.data.all_objects
            this.editor.updateData(this.objectData)
          }
        } else {
          this.$message.info(res.message)
        }
      })
    }
  }
}
</script>

<style lang="less" scoped>
.screen-container {
  width: 100vw;
  height: 100vh;
  background-color: #f4f7f9;
}
</style>
<style>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s;
}
.fade-enter,
.fade-leave-active {
  opacity: 0;
}
.fade-move {
  transition: transform 0.25s;
}
.text-ellipsis {
  display: inline-block;
  overflow: hidden;
  text-overflow: ellipsis;
  vertical-align: bottom;
  white-space: nowrap;
}
</style>
