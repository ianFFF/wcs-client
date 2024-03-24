<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="page-content">
        <table-tools :actions="['excel']" @onExcel="onExcel">
          <template slot="ex-before">
            <a-range-picker
              v-model="searchTime"
              valueFormat="YYYY-MM-DD HH:mm"
              :placeholder="['日志开始时间', '日志结束时间']"
              style="width: 225px; margin-right: 8px"
            />

            <a-select
              v-model="alarm_code"
              placeholder="请选择异常编码"
              style="width: 225px; margin-right: 8px"
              :allowClear="true"
            >
              <a-select-option v-for="item in alarmCodeList" :key="item.value" :value="item.value">
                {{ item.label }}
              </a-select-option>
            </a-select>

            <a-select
              v-model="alarm_type"
              placeholder="请选择异常类型"
              style="width: 225px; margin-right: 8px"
              :allowClear="true"
            >
              <a-select-option v-for="item in alarmTypeList" :key="item.value" :value="item.value">
                {{ item.label }}
              </a-select-option>
            </a-select>

            <a-button type="primary" @click="onSearch">搜索</a-button>
          </template>
        </table-tools>

        <a-table
          class="main-table"
          :columns="columns"
          :data-source="data"
          rowKey="id"
          bordered
          :pagination="tablePagination"
          @change="tableChange"
          :loading="loading"
        >
          <template slot="alarm_level" slot-scope="record">
            <a-tag v-if="record === '高'" color="red">高</a-tag>
            <a-tag v-if="record === '中'" color="orange">中</a-tag>
            <a-tag v-if="record === '低'" color="green">低</a-tag>
          </template>
        </a-table>
      </div>
    </template>
  </page-header-wrapper>
</template>

<script>
import { PageHeaderWrapper } from '@/components/ProLayout'
import TableTools from '@/components/TableTools'
import moment from 'moment'
import { getErrorLogsList, GetAlarmType, GetAlarmCode } from '@/api/wcs/equipment_logs'

export default {
  name: 'Logs',
  components: {
    PageHeaderWrapper,
    TableTools
  },
  data() {
    return {
      loading: false,
      tablePagination: {
        // pageSizeOptions: ['10', '20'],
        // showSizeChanger: true,
        total: 0
      },
      pageInfo: {
        PageNum: 1,
        PageSize: 10
      },
      columns: [
        {
          title: '日志信息',
          dataIndex: 'message'
        },
        {
          title: '异常编码',
          dataIndex: 'alarm_code'
        },
        {
          title: '异常等级',
          dataIndex: 'alarm_level',
          scopedSlots: { customRender: 'alarm_level' }
        },
        {
          title: '异常类型',
          dataIndex: 'alarm_type'
        },
        {
          title: '日志时间',
          dataIndex: 'createTime'
        }
      ],
      data: [],
      alarmCodeList: [],
      alarmTypeList: [],
      alarm_code: undefined,
      alarm_type: undefined,
      searchTime: undefined
    }
  },
  created() {},
  mounted() {
    GetAlarmCode().then((res) => {
      if (res.code === 200) {
        this.alarmCodeList = res.data.map((item) => {
          return {
            value: item.alarm_code,
            label: item.alarm_code
          }
        })
      }
    })
    GetAlarmType().then((res) => {
      if (res.code === 200) {
        this.alarmTypeList = res.data.map((item) => {
          return {
            value: item.alarm_type,
            label: item.alarm_type
          }
        })
      }
    })

    this.getData()
  },
  methods: {
    tableChange(pagination, filters, sorter, { currentDataSource }) {
      this.pageInfo.PageNum = pagination.current
      this.pageInfo.PageSize = pagination.pageSize
      this.getData()
    },
    onSearch(val) {
      this.pageInfo.PageNum = 1
      this.pageInfo.PageSize = 10
      this.getData(val)
    },
    onExcel() {
      const time = {
        SearchBeginTime: '',
        SearchEndTime: ''
      }
      if (this.searchTime && this.searchTime.length === 2) {
        time.SearchBeginTime = this.searchTime[0]
        time.SearchEndTime = this.searchTime[1]
      }
      this.download(
        '/EquipmentRecords/ExportExcel',
        {
          id: '',
          alarm_level: '',
          alarm_code: this.alarm_code || 'error',
          alarm_type: this.alarm_type,
          message: '',
          createTime: '',
          ...time
        },
        `故障日志_${moment().format('YYYYMMDDHHmmss')}.xlsx`
      )
    },
    getData(searchKey) {
      this.loading = true

      const time = {
        SearchBeginTime: '',
        SearchEndTime: ''
      }
      if (this.searchTime && this.searchTime.length === 2) {
        time.SearchBeginTime = this.searchTime[0]
        time.SearchEndTime = this.searchTime[1]
      }
      getErrorLogsList({
        ...this.pageInfo,
        ...time,
        alarm_code: this.alarm_code || 'error',
        alarm_type: this.alarm_type
      }).then((res) => {
        if (res.code === 200) {
          this.data = res.data.data
          this.tablePagination.total = res.data.total
        }

        this.loading = false
      })
    }
  }
}
</script>

<style lang="less" scoped></style>
