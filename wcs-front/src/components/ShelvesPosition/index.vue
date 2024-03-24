<template>
  <div>
    <a-modal v-model="visible" :title="title" @ok="handleOk" :maskClosable="false" :width="1500">
      <template slot="footer">
        <a-button key="cancel" @click="handleOk('cancel')">取消</a-button>
        <a-button key="submit" type="primary" :loading="loading" @click="handleOk('submit')">确定</a-button>
      </template>
      <a-spin tip="Loading..." :spinning="loading">
        <a-form-model ref="ruleForm">
          <div class="shelves">
            <a-space direction="vertical">
              <div class="shelves-row" v-for="(row, rowIndex) in rows" :key="rowIndex">
                <a-space>
                  <a-popover
                    placement="rightTop"
                    v-for="(col, colIndex) in cols"
                    :key="colIndex"
                    v-if="getPosition({ row, col }).is_loaded"
                  >
                    <template #content>
                      <div class="pop-tip">
                        <span>任务编号</span>
                        <p>{{ getPosition({ row, col }).task_code }}</p>
                      </div>
                      <div class="pop-tip">
                        <span>托盘码</span>
                        <p>{{ getPosition({ row, col }).loader_code }}</p>
                      </div>
                      <div class="pop-tip">
                        <span>完成时间</span>
                        <p>{{ getPosition({ row, col }).completeTime }}</p>
                      </div>
                    </template>
                    <template #title>
                      <span>货位信息</span>
                    </template>
                    <div
                      :class="{
                        'shelves-position': true
                      }"
                      @click="onPositionSelected(getPosition({ row, col }).id)"
                    >
                      <a-icon type="check" v-if="getPosition({ row, col }).id === selectedPosition" />
                      {{ `${row}-${col}` }}
                      <span class="position-loaded">载货</span>
                    </div>
                  </a-popover>
                  <div
                    v-else
                    :class="{
                      'shelves-position': true,
                      selected: getPosition({ row, col }).id === selectedPosition,
                      dj: getPosition({ row, col }).status == 1,
                      yc: getPosition({ row, col }).status == 2
                    }"
                    @click="onPositionSelected(getPosition({ row, col }).id)"
                  >
                    <a-icon type="check" v-if="getPosition({ row, col }).id === selectedPosition" />
                    {{ `${row}-${col}` }}
                    <span class="position-error" v-if="getPosition({ row, col }).status !== 0">
                      {{ getPosition({ row, col }).status == 1 ? '冻结' : '异常' }}
                    </span>
                    <span class="position-loaded" v-else-if="getPosition({ row, col }).is_loaded">载货</span>
                    <span class="position-locked" v-else-if="getPosition({ row, col }).is_locked">锁定</span>
                    <span class="position-free" v-else>空闲</span>
                  </div>
                </a-space>
              </div>
            </a-space>
          </div>
        </a-form-model>
      </a-spin>
    </a-modal>
  </div>
</template>
<script>
import { shelevsPositionList } from '@/api/wcs/shelves'

export default {
  name: 'ShelvesPositionForm',
  props: {
    type: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      visible: false,
      loading: false,
      title: '货架信息',

      selectedPosition: '',
      data: {}
    }
  },
  computed: {
    rows() {
      const r = []
      for (var i = this.data.shelves_rows_num - 1; i >= 0; i--) {
        r.push(i + 1)
      }

      return r
    },
    cols() {
      const c = []
      for (var i = this.data.shelves_cols_num - 1; i >= 0; i--) {
        c.push(i + 1)
      }

      return c
    }
  },
  methods: {
    getPosition(value) {
      const index = this.data.shelvesPosition.findIndex((x) => x.col === value.col && x.row === value.row)
      if (index >= 0) {
        return this.data.shelvesPosition[index]
      }
      return { id: '' }
    },
    showModal(id) {
      if (id) {
        this.getData(id)
      }
      this.selectedPosition = ''
      this.visible = true
    },
    /**
     * 点位选择
     */
    onPositionSelected(id) {
      const index = this.data.shelvesPosition.findIndex((x) => x.id === id)
      if (index >= 0) {
        const position = this.data.shelvesPosition[index]

        if (this.type === 'rk' && (position.is_loaded || position.is_locked || position.status !== 0)) {
          this.$message.info('已载货、已被任务锁定、非空闲的货位不可选择')

          return
        } else if (this.type === 'ck' && !position.is_loaded) {
          this.$message.info('未载货货位不可选择')

          return
        } else if (this.type === 'lock' && (position.is_loaded || position.is_locked)) {
          this.$message.info('已载货、已被任务锁定的货位不可选择')

          return
        }

        this.selectedPosition = this.selectedPosition === id ? '' : id
      }
    },
    handleOk(e) {
      this.visible = false

      if (e === 'cancel') {
        return
      }

      const index = this.data.shelvesPosition.findIndex((x) => x.id === this.selectedPosition)

      this.$emit('onFinish', { type: e, data: this.data.shelvesPosition[index], shelves_code: this.data.shelves_code })
    },
    getData(shelvesId) {
      this.loading = true
      shelevsPositionList({ shelvesId }).then((res) => {
        console.log(res)
        this.title = `${res.data.shelves_code}货架信息`
        this.data = res.data
        this.loading = false
      })
    }
  }
}
</script>
<style lang="less" scoped>
.pop-tip {
  width: 300px;
  display: flex;
  justify-content: space-between;
  color: #999;
  span {
    font-weight: bold;
  }
}

.shelves {
  overflow-x: scroll;
  .shelves-row {
    display: flex;
    .shelves-position {
      height: 100px;
      width: 100px;
      line-height: 100px;
      text-align: center;
      background: #f0dbb0;
      cursor: pointer;
    }
    .shelves-position.selected {
      background: #91bcbb;
    }
    .shelves-position.dj {
      background: #a7ecf2;
    }
    .shelves-position.yc {
      background: #ff8000;
    }
    .position-free {
      color: #333333;
    }
    .position-error,
    .position-loaded,
    .position-locked {
      color: #d9282d;
      font-weight: bold;
    }
  }
}
</style>
