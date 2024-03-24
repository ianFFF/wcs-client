<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="page-content">
        <table-tools @onAdd="onAdd" @onSearch="onSearch" />

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
              <a @click="onRowEdit(record, 'delete')">删除</a>
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

export default {
  name: 'Task',
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
        showSizeChanger: true
      },
      // columns: [
      //   {
      //     title: '设备类型',
      //     dataIndex: 'name',
      //     scopedSlots: { customRender: 'name' }
      //   },
      //   {
      //     title: '设备名称',
      //     className: 'column-money',
      //     dataIndex: 'money'
      //   },
      //   {
      //     title: '供应商',
      //     dataIndex: 'address'
      //   }
      // ],
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
      data: [
        {
          id: '1',
          shelves_code: 'HJ001',
          shelves_cols_num: '5',
          shelves_rows_num: '4',
          shelves_deep_num: '1',
          max_load: '20',
          upper_system_id: '',
          warehouse_area: 'A库区'
        },
        {
          id: '2',
          shelves_code: 'HJ002',
          shelves_cols_num: '5',
          shelves_rows_num: '4',
          shelves_deep_num: '1',
          max_load: '20',
          upper_system_id: '',
          warehouse_area: 'A库区'
        },
        {
          id: '3',
          shelves_code: 'HJ003',
          shelves_cols_num: '10',
          shelves_rows_num: '3',
          shelves_deep_num: '2',
          max_load: '10',
          upper_system_id: '',
          warehouse_area: 'B库区'
        },
        {
          id: '4',
          shelves_code: 'HJ004',
          shelves_cols_num: '10',
          shelves_rows_num: '3',
          shelves_deep_num: '2',
          max_load: '10',
          upper_system_id: '',
          warehouse_area: 'B库区'
        }
      ]
    }
  },
  created() {},
  mounted() {},
  methods: {
    tableChange(pagination, filters, sorter, { currentDataSource }) {
      console.log(pagination, filters, sorter, currentDataSource)
    },
    onSearch(val) {
      console.log('on search ' + val)
    },
    onAdd() {
      console.log('on click')
      this.$refs.editForm.showModal('')
    },
    onRowEdit(record, type) {
      console.log(`edit record:${type}`, record)
      if (type === 'edit') {
        this.$refs.editForm.showModal(record.id)
      }
    },
    onEditFormFinish(e) {
      console.log(e)
      if (e.type === 'submit') {
        this.loading = true
        setTimeout(() => {
          this.loading = false
        }, 1000)
      }
    }
  }
}
</script>

<style lang="less" scoped></style>
