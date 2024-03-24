<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="page-content">
        <table-tools :actions="['search', 'add']" @onAdd="onAdd" @onSearch="onSearch" />

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
          <template slot="logs_interval_type" slot-scope="record">
            <span v-if="record == 0"> 天 </span>
            <span v-else-if="record == 1"> 月 </span>
          </template>
          <template slot="logs_is_record" slot-scope="record">
            <a-switch :checked="record" checked-children="开" un-checked-children="关" disabled />
          </template>
          <template slot="action" slot-scope="record">
            <a-space>
              <a @click="onRowEdit(record, 'edit')">编辑</a>
              <a-popconfirm
                title="删除后数据不可恢复，确认操作！"
                @confirm="onRowEdit(record, 'delete')"
                ok-text="确定"
                cancel-text="取消"
              >
                <a href="#">删除</a>
              </a-popconfirm>
            </a-space>
          </template>
        </a-table>
      </div>
    </template>
    <equipment-form ref="editForm" @onFinish="onEditFormFinish" />
  </page-header-wrapper>
</template>

<script>
import { PageHeaderWrapper } from '@/components/ProLayout'
import TableTools from '@/components/TableTools'
import EquipmentForm from './components/form'
import { getPageList, deleteData } from '@/api/wcs/equipment_logs'

export default {
  name: 'Logs',
  components: {
    PageHeaderWrapper,
    TableTools,
    EquipmentForm
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
          title: '日志设备',
          dataIndex: 'equipment_name'
        },
        {
          title: '日志周期',
          dataIndex: 'logs_interval'
        },
        {
          title: '周期单位',
          dataIndex: 'logs_interval_type',
          scopedSlots: { customRender: 'logs_interval_type' }
        },
        {
          title: '是否启用',
          dataIndex: 'logs_is_record',
          scopedSlots: { customRender: 'logs_is_record' }
        },
        {
          title: '操作',
          // dataIndex: 'name',
          scopedSlots: { customRender: 'action' }
        }
      ],
      data: []
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
    onAdd() {
      console.log('on click')
      this.$refs.editForm.showModal('')
    },
    onRowEdit(record, type) {
      console.log(`edit record:${type}`, record, record.logs_is_record === 1)
      if (type === 'edit') {
        this.$refs.editForm.showModal(record.id)
      } else if (type === 'delete') {
        this.loading = true
        deleteData([record.id]).then((res) => {
          if (res.code === 200) {
            this.$message.success('操作成功')
            this.loading = false
            this.getData()
          }
        })
      }
    },
    onEditFormFinish(e) {
      console.log(e)
      if (e.type === 'submit') {
        this.pageInfo.PageNum = 1
        this.pageInfo.PageSize = 10
        this.getData()
      }
    },
    getData(searchKey) {
      this.loading = true
      getPageList({ ...this.pageInfo, equipment_name: searchKey }).then((res) => {
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
