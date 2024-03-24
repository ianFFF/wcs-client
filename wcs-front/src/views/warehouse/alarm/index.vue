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
          <template slot="alarm_level" slot-scope="record">
            <a-tag v-if="record === '高'" color="red">高</a-tag>
            <a-tag v-if="record === '中'" color="orange">中</a-tag>
            <a-tag v-if="record === '低'" color="green">低</a-tag>
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
// import { getUserProfile } from '@/api/system/user'
import { getPageList, deleteData } from '@/api/wcs/equipment_alarm'

export default {
  name: 'Alarm',
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
        // showSizeChanger: true
        total: 0
      },
      pageInfo: {
        PageNum: 1,
        PageSize: 10
      },
      columns: [
        {
          title: '设备类型',
          dataIndex: 'equipment_type'
        },
        {
          title: '设备名称',
          dataIndex: 'equipment_name'
        },
        {
          title: '异常等级',
          dataIndex: 'alarm_level',
          scopedSlots: { customRender: 'alarm_level' }
        },
        {
          title: '异常编码',
          dataIndex: 'alarm_code'
        },
        {
          title: '异常说明',
          dataIndex: 'alarm_remark'
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
  computed: {
    // ...mapGetters(['avatar', 'nickname'])
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
