<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="page-content">
        <table-tools :actions="['search']" @onSearch="onSearch">
          <template slot="ex-before">
            <a-select v-model="action_type" placeholder="请选择操作类型" style="width: 225px; margin-right: 8px">
              <a-select-option v-for="item in actionTypeList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
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
          <template slot="status" slot-scope="record">
            <a-tag v-if="record === '0'" color="red">等待中</a-tag>
            <a-tag v-if="record === '1'" color="orange">进行中</a-tag>
            <a-tag v-if="record === '2'" color="green">已完成</a-tag>
          </template>
          <template slot="action" slot-scope="record">
            <a-space>
              <a @click="onRowEdit(record, 'detail')">详情</a>
            </a-space>
          </template>
        </a-table>
      </div>
    </template>
  </page-header-wrapper>
</template>

<script>
import { PageHeaderWrapper } from '@/components/ProLayout'
import TableTools from '@/components/TableTools'
// import { getUserProfile } from '@/api/system/user'

export default {
  name: 'EquipmentNotice',
  components: {
    PageHeaderWrapper,
    TableTools
  },
  data() {
    return {
      loading: false,
      tablePagination: {
        // pageSizeOptions: ['10', '20'],
        showSizeChanger: true
      },
      action_type: undefined,
      actionTypeList: [
        { label: '入库', value: '1' },
        { label: '出库', value: '2' },
        { label: '搬库', value: '3' }
      ],
      columns: [
        {
          title: '任务号',
          dataIndex: 'task_code'
        },
        {
          title: '货架条码',
          dataIndex: 'shelves_qr_code'
        },
        {
          title: '操作类型',
          dataIndex: 'action_type'
        },
        {
          title: '重量(kg)',
          dataIndex: 'weight'
        },
        {
          title: '尺寸',
          dataIndex: 'size'
        },
        {
          title: '状态',
          dataIndex: 'status',
          scopedSlots: { customRender: 'status' }
        },
        {
          title: '任务时间',
          dataIndex: 'task_create_time'
        },
        {
          title: '持续时间(min)',
          dataIndex: 'task_time_use'
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
          task_code: 'A00001',
          shelves_qr_code: 'HHHHFFF00021',
          action_type: '出库',
          weight: '20',
          size: '',
          status: '0',
          task_create_time: '2022-12-19 17:00',
          task_time_use: '2'
        },
        {
          id: '2',
          task_code: 'A00002',
          shelves_qr_code: 'HHHHFFF00003',
          action_type: '出库',
          weight: '20',
          size: '',
          status: '1',
          task_create_time: '2022-12-19 17:00',
          task_time_use: '2'
        },
        {
          id: '3',
          task_code: 'A00003',
          shelves_qr_code: 'HHHHFFF00001',
          action_type: '入库',
          weight: '20',
          size: '',
          status: '2',
          task_create_time: '2022-12-18 17:00',
          task_time_use: '2'
        }
      ]
    }
  },
  computed: {
    // ...mapGetters(['avatar', 'nickname'])
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
