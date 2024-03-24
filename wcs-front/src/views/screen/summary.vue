<template>
  <div class="summary-container">
    <div class="box">
      <div class="title">周统计</div>
      <div class="num-list">
        <div class="num-box">
          <div class="num" style="color: #2a9d8f">
            {{ monthStatistics.rkCount }}
          </div>
          <div class="remark">入库</div>
        </div>
        <div class="num-box">
          <div class="num" style="color: #e76f51">{{ monthStatistics.ckCount }}</div>
          <div class="remark">出库</div>
        </div>
      </div>
    </div>
    <div class="box">
      <div class="title">月统计</div>
      <div class="num-list">
        <div class="num-box">
          <div class="num" style="color: #2a9d8f">
            {{ weeklyStatistics.rkCount }}
          </div>
          <div class="remark">入库</div>
        </div>
        <div class="num-box">
          <div class="num" style="color: #e76f51">
            {{ weeklyStatistics.ckCount }}
          </div>
          <div class="remark">出库</div>
        </div>
      </div>
    </div>
    <div class="box">
      <div class="title">年统计</div>
      <div class="num-list">
        <div class="num-box">
          <div class="num" style="color: #2a9d8f">
            {{ yearStatistics.rkCount }}
          </div>
          <div class="remark">入库</div>
        </div>
        <div class="num-box">
          <div class="num" style="color: #e76f51">
            {{ yearStatistics.ckCount }}
          </div>
          <div class="remark">出库</div>
        </div>
      </div>
    </div>
    <div class="box" style="width: 280px">
      <div class="title">设备状态</div>
      <vue-seamless-scroll :data="equipmentStatus || []" class="event-list" :class-option="classOption">
        <div class="row" v-for="(item, key) of equipmentStatus" :key="key">
          <div class="col text-ellipsis" style="flex: 1">{{ item.name }}</div>
          <div class="col">
            <span class="ant-tag css-acm2ia" :class="item.is_connected ? 'ant-tag-green' : 'ant-tag-red'">{{
              item.is_connected ? '已连接' : '未连接'
            }}</span>
          </div>
          <div class="col">
            <span
              class="ant-tag ant-tag-has-color css-acm2ia"
              style="margin-right: 0px"
              :style="{ 'background-color': item.is_busy ? 'rgb(255, 85, 0)' : 'rgb(135, 208, 104)' }"
            >{{ item.is_busy ? '繁忙' : '空闲' }}</span
            >
          </div>
        </div>
      </vue-seamless-scroll>
      <!-- <div class="event-list">

      </div> -->
    </div>
  </div>
</template>

<script>
import { taskStatistics, getEquipmentStatus } from '@/api/wcs/task'
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
      taskStatisticsData: null,
      equipmentStatus: null,
      intervalGetData: null
    }
  },
  computed: {
    monthStatistics() {
      if (this.taskStatisticsData) {
        return this.taskStatisticsData.monthStatistics
      } else {
        return { rkCount: 0, ckCount: 0 }
      }
    },
    weeklyStatistics() {
      if (this.taskStatisticsData) {
        return this.taskStatisticsData.weeklyStatistics
      } else {
        return { rkCount: 0, ckCount: 0 }
      }
    },
    yearStatistics() {
      if (this.taskStatisticsData) {
        return this.taskStatisticsData.yearStatistics
      } else {
        return { rkCount: 0, ckCount: 0 }
      }
    }
  },
  mounted() {
    this.getTaskStatistics()
    this.handleGetEquipmentStatus()
    this.intervalGetData = setInterval(() => {
      this.getTaskStatistics()
      this.handleGetEquipmentStatus()
    }, 5000)
  },
  beforeDestroy() {
    if (this.intervalGetData) {
      clearInterval(this.intervalGetData)
    }
  },
  methods: {
    getTaskStatistics() {
      taskStatistics().then((res) => {
        this.loading = false
        if (res.status) {
          this.taskStatisticsData = res.data
        } else {
          this.$message.info(res.message)
        }
      })
    },
    handleGetEquipmentStatus() {
      getEquipmentStatus().then((res) => {
        this.loading = false
        if (res.status) {
          this.equipmentStatus = res.data.equipments
        } else {
          this.$message.info(res.message)
        }
      })
    }
  }
}
</script>

<style lang="less" scoped>
.summary-container {
  position: absolute;
  top: 0;
  left: 0;
  z-index: 999;
  display: inline-flex;
  justify-content: center;
  width: 100%;
  margin-top: 24px;
  .box {
    width: 200px;
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
    .num-list {
      display: inline-flex;
      width: 100%;
      .num-box {
        flex: 1;
        text-align: center;
        padding: 16px;
        .num {
          // min-height: 36px;
          font-size: 24px;
          font-weight: bold;
        }
        .remark {
          color: #7d7c83;
        }
      }
    }
    .event-list {
      padding: 16px 8px;

      height: 81px;
      width: 100%;
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
        }
      }
    }
  }
}
</style>
