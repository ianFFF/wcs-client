<template>
  <div class="footer-container">
    <div class="box">
      <div class="title">状态信息</div>
      <vue-seamless-scroll :data="serverInfoMessages || []" class="event-list" :class-option="classOption">
        <div class="row" v-for="(item, key) of serverInfoMessages" :key="key">
          <div class="col text-ellipsis">{{ item.message }}</div>
        </div>
      </vue-seamless-scroll>
    </div>
    <div class="box">
      <div class="title">任务信息</div>
      <div class="event-list">
        <div class="row" v-for="(item, key) of taskData" :key="key">
          <div class="col text-ellipsis">{{ item }}</div>
        </div>
      </div>
    </div>
    <div class="box">
      <div class="title">故障信息</div>
      <vue-seamless-scroll :data="serverInfoMessages || []" class="event-list" :class-option="classOption">
        <div
          class="row message"
          v-for="(item, key) of serverErrorMessages"
          :key="key"
          @click="onShowMessage(item.message)"
        >
          <div class="col text-ellipsis">{{ item.message }}</div>
        </div>
      </vue-seamless-scroll>
    </div>

    <a-modal title="故障信息" width="600px" :visible="messageShow" @cancel="messageShow = false" :footer="null">
      <a-result status="warning" :title="messageTitle"> </a-result>
    </a-modal>
  </div>
</template>

<script>
import { getTop3Task, getServerMessages } from '@/api/wcs/task'
import vueSeamlessScroll from 'vue-seamless-scroll'

export default {
  components: {
    vueSeamlessScroll
  },
  data() {
    return {
      classOption: {
        step: 0.2
      },
      taskData: [],
      serverInfoMessages: [],
      serverErrorMessages: [],
      intervalGetData: null,
      messageShow: false,
      messageTitle: ''
    }
  },
  mounted() {
    this.onGetTop3Task()
    this.onGetServerInfo()
    this.onGetServerError()
    this.intervalGetData = setInterval(() => {
      this.onGetTop3Task()
      this.onGetServerInfo()
      this.onGetServerError()
    }, 5000)
  },
  beforeDestroy() {
    if (this.intervalGetData) {
      clearInterval(this.intervalGetData)
    }
  },
  methods: {
    onGetTop3Task() {
      this.loading = true
      getTop3Task().then((res) => {
        this.loading = false
        if (res.status) {
          this.taskData = res.data.messages
        } else {
          this.$message.info(res.message)
        }
      })
    },
    onGetServerInfo() {
      this.loading = true
      getServerMessages({ type: 'INFO' }).then((res) => {
        this.loading = false
        if (res.status) {
          this.serverInfoMessages = res.data
        } else {
          this.$message.info(res.message)
        }
      })
    },
    onGetServerError() {
      this.loading = true
      getServerMessages({ type: 'ERROR' }).then((res) => {
        this.loading = false
        if (res.status) {
          this.serverErrorMessages = res.data
        } else {
          this.$message.info(res.message)
        }
      })
    },
    onShowMessage(message) {
      this.messageShow = true
      this.messageTitle = message
    }
  }
}
</script>

<style lang="less" scoped>
.footer-container {
  position: absolute;
  bottom: 0;
  left: 0;
  z-index: 999;
  display: inline-flex;
  justify-content: center;
  width: 100%;
  margin-bottom: 24px;
  .box {
    width: 302px;
    margin: 16px;
    color: rgba(0, 0, 0, 0.85);
    padding: 16px;
    border-radius: 4px;
    background-color: #fff;
    box-shadow: 0px 6px 16px -8px rgba(0, 0, 0, 0.08), 0px 9px 28px 0px rgba(0, 0, 0, 0.05),
      0px 12px 48px 16px rgba(0, 0, 0, 0.03);
    .title {
      font-size: 16px;
      font-family: PingFangSC-Medium, PingFang SC;
      font-weight: 500;
      color: rgba(0, 0, 0, 0.85);
      line-height: 22px;
      display: flex;
      align-items: center;
      &::before {
        content: ' ';
        width: 3px;
        height: 16px;
        background: #1890ff;
        margin-right: 8px;
      }
    }
    .event-list {
      padding: 16px 8px;
      height: 94px;
      width: 280px;
      margin: 0 auto;
      overflow: hidden;
      .row {
        font-size: 12px;
        line-height: 22px;
        margin-bottom: 4px;
        width: 100%;
        display: inline-flex;
        .col {
          color: 12px;
          width: 100%;
        }
      }
      .message {
        cursor: pointer;
      }
    }
  }
}
</style>
