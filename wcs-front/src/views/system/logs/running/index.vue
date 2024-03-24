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
        </a-table>
      </div>
    </template>
  </page-header-wrapper>
</template>

<script>
import { PageHeaderWrapper } from '@/components/ProLayout'
import TableTools from '@/components/TableTools'
import moment from 'moment'
import { getRunningLogsList } from '@/api/wcs/equipment_logs'

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
          title: '日志时间',
          dataIndex: 'createTime'
        },
        {
          title: '任务流水号',
          dataIndex: 'task_code'
        },
        {
          title: '托盘码',
          dataIndex: 'loader_code'
        },
        {
          title: '执行结果',
          dataIndex: 'task_status'
        },
        {
          title: '任务发起时间',
          dataIndex: 'taskCreateTime'
        },
        {
          title: '任务完成时间',
          dataIndex: 'taskCompleteTime'
        }
      ],
      data: [],
      searchTime: undefined
    }
  },
  created() {},
  mounted() {
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
          alarm_code: '',
          message: '',
          createTime: '',
          ...time
        },
        `运行日志_${moment().format('YYYYMMDDHHmmss')}.xlsx`
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

      getRunningLogsList({ ...this.pageInfo, ...time }).then((res) => {
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
