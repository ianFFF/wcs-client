<template>
  <div class="front-view-container" v-if="data && data.shelvesPosition">
    <div class="row" v-for="(row, key) of rows" :key="key">
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
          class="col"
          :class="{
            'status-loaded': true
          }"
          style="line-height: 100px"
        >
          {{ `${row}-${col}载货` }}
        </div>
      </a-popover>

      <div
        v-else
        class="col"
        :class="{
          'status-loaded': getPosition({ row, col }).is_loaded,
          'status-dj': getPosition({ row, col }).status == 1,
          'status-yc': getPosition({ row, col }).status == 2
        }"
        style="line-height: 100px"
      >
        {{
          getPosition({ row, col }).status == 0
            ? `${row}-${col} ${getPosition({ row, col }).is_loaded ? '载货' : '空闲'}`
            : `${row}-${col} ${getPosition({ row, col }).status == 1 ? '冻结' : '异常'}`
        }}
      </div>
    </div>
  </div>
</template>

<script>
import { shelevsPositionList } from '@/api/wcs/shelves'
export default {
  props: {
    shelvesId: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      data: {}
    }
  },
  mounted() {
    this.getData()
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
    getData() {
      this.loading = true
      shelevsPositionList({ shelvesId: this.shelvesId }).then((res) => {
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

.front-view-container {
  width: 100%;
  height: 100%;
  overflow: auto;
}
.row {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  .col {
    background-color: #f2f2f2;
    border: 1px solid #333;
    padding: 4px;
    height: 100px;
    width: 100px;
    border-bottom: 1px solid transparent;
    border-right: 1px solid transparent;
    text-align: center;
    &:last-of-type {
      border-right: 1px solid #333;
    }
  }
  &:last-of-type {
    .col {
      border-bottom: 1px solid #333;
    }
  }
}
// .status-loaded {
//   background-color: #cfd3bc !important;
//   border-color: #fff;
// }
.status-loaded {
  background-color: #f7dc66 !important;
  border-color: #f8e8ab;
}
// .status-100 {
//   background-color: #f1184c !important;
//   border-color: #fff;
// }
.status-dj {
  background-color: #a7ecf2 !important;
  border-color: #f8e8ab;
}
.status-yc {
  background-color: #f7dc66 !important;
  border-color: #ff8000;
}
</style>
