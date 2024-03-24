<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="plan">
        <a-card title="数据初始化" style="width: 300px">
          <div class="page-content">
            <a-button type="primary" :loading="loading" @click="onInitAllData">仓库数据初始化</a-button>
          </div>
        </a-card>

        <a-card title="任务处理" style="width: 300px">
          <div class="page-content">
            <a-button type="primary" :loading="loading" @click="onStartRk">开启自动入库</a-button>
            <a-button type="primary" :loading="loading" @click="onStopRk">关闭自动入库</a-button>
            <a-button type="primary" :loading="loading" @click="onStartTask">开启自动处理任务</a-button>
            <a-button type="primary" :loading="loading" @click="onStopTask">关闭自动处理任务</a-button>
          </div>
        </a-card>

        <a-card title="demo" style="width: 300px">
          <div class="page-content">
            <a-button type="primary" :loading="loading" @click="onAllObject">(大屏)获取所有设备信息</a-button>
            <a-button type="primary" :loading="loading" @click="onTaskStatistics">(大屏)获取任务统计信息</a-button>
            <a-button type="primary" :loading="loading" @click="onGetServerInfo">(大屏)获取状态信息</a-button>
            <a-button type="primary" :loading="loading" @click="onGetTop3Task">(大屏)获取任务信息</a-button>
            <a-button type="primary" :loading="loading" @click="onGetServerError">(大屏)获取故障信息</a-button>
            <a-button type="primary" :loading="loading" @click="onGetEquipmentStatus">(大屏)获取设备连接信息</a-button>
          </div>
        </a-card>
      </div>
    </template>
  </page-header-wrapper>
</template>

<script>
import { autoRkModeStart, autoRkModeStop, dealTaskStart, dealTaskStop, allObject } from '@/api/wcs/actions'
import { initAllData, taskStatistics, getServerMessages, getTop3Task, getEquipmentStatus } from '@/api/wcs/task'

export default {
  name: 'Demo',
  components: {},
  data() {
    return {
      loading: false
    }
  },
  computed: {
    // ...mapGetters(['avatar', 'nickname'])
  },
  created() {},
  mounted() {},
  methods: {
    onStartRk() {
      this.loading = true
      autoRkModeStart().then((res) => {
        this.loading = false
        this.$message.success('操作成功')
      })
    },
    onStopRk() {
      this.loading = true
      autoRkModeStop().then((res) => {
        this.loading = false
        this.$message.success('操作成功')
      })
    },
    onStartTask() {
      this.loading = true
      dealTaskStart().then((res) => {
        this.loading = false
        this.$message.success('操作成功')
      })
    },
    onStopTask() {
      this.loading = true
      dealTaskStop().then((res) => {
        this.loading = false
        this.$message.success('操作成功')
      })
    },
    onInitAllData() {
      this.loading = true
      initAllData().then((res) => {
        this.loading = false
        this.$message.success('操作成功')
      })
    },
    onAllObject() {
      this.loading = true
      allObject().then((res) => {
        this.loading = false
        if (res.status) {
          this.$message.success('操作成功,F12查看')
        } else {
          this.$message.info(res.message)
        }
      })
    },
    onTaskStatistics() {
      this.loading = true
      taskStatistics().then((res) => {
        this.loading = false
        if (res.status) {
          this.$message.success('操作成功,F12查看')
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
          this.$message.success('操作成功,F12查看')
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
          this.$message.success('操作成功,F12查看')
        } else {
          this.$message.info(res.message)
        }
      })
    },
    onGetTop3Task() {
      this.loading = true
      getTop3Task().then((res) => {
        this.loading = false
        if (res.status) {
          this.$message.success('操作成功,F12查看')
        } else {
          this.$message.info(res.message)
        }
      })
    },
    onGetEquipmentStatus() {
      this.loading = true
      getEquipmentStatus().then((res) => {
        this.loading = false
        if (res.status) {
          this.$message.success('操作成功,F12查看')
        } else {
          this.$message.info(res.message)
        }
      })
    }
  }
}
</script>

<style lang="less" scoped>
.plan {
  display: flex;
  justify-content: space-around;
  .page-content {
    display: flex;
    flex-direction: column;
    margin-bottom: 24px;
    button {
      margin: 8px;
    }
  }
}
</style>
