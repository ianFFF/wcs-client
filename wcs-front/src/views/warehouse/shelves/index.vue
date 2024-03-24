<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="page-content">
        <table-tools :actions="['search', 'add']" @onAdd="onAdd" @onSearch="onSearch">
          <template slot="ex-after">
            <a-button class="btn" type="primary" @click="onPositionLock">货位锁定</a-button>
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
    <position-form ref="positionForm" @onFinish="onPositionFormFinish" />
  </page-header-wrapper>
</template>

<script>
import { PageHeaderWrapper } from '@/components/ProLayout'
import TableTools from '@/components/TableTools'
import EquipmentForm from './components/form'
import PositionForm from './components/positionForm'
import { getPageList, deleteData } from '@/api/wcs/shelves'

export default {
  name: 'Shelves',
  components: {
    PageHeaderWrapper,
    TableTools,
    EquipmentForm,
    PositionForm
  },
  data() {
    return {
      loading: false,
      tablePagination: {
        // pageSizeOptions: ['10', '20'],
        // showSizeChanger: true
        total: 0
      },
      pageInfo: {
        PageNum: 1,
        PageSize: 10
      },
      columns: [
        {
          title: '货架编号',
          dataIndex: 'shelves_code'
        },
        {
          title: '货架列数',
          dataIndex: 'shelves_cols_num'
        },
        {
          title: '货架层数',
          dataIndex: 'shelves_rows_num'
        },
        {
          title: '货架深度',
          dataIndex: 'shelves_deep_num'
        },
        {
          title: '每层最大承重（KG）',
          dataIndex: 'max_load'
        },
        {
          title: '库区',
          dataIndex: 'warehouse_area'
        },
        {
          title: '上游系统ID',
          dataIndex: 'upper_system_id'
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
      this.$refs.editForm.showModal('')
    },
    onRowEdit(record, type) {
      console.log(`edit record:${type}`, record)
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
      if (e.type === 'submit') {
        this.pageInfo.PageNum = 1
        this.pageInfo.PageSize = 10
        this.getData()
      }
    },
    onPositionFormFinish(e) {
      console.log(e)
    },
    getData(searchKey) {
      this.loading = true
      getPageList({ ...this.pageInfo, shelves_code: searchKey }).then((res) => {
        if (res.code === 200) {
          this.data = res.data.data
          this.tablePagination.total = res.data.total
        }

        this.loading = false
      })
    },
    onPositionLock() {
      this.$refs.positionForm.showModal()
    }
  }
}
</script>

<style lang="less" scoped></style>
