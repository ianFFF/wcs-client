<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="page-content">
        <table-tools :actions="['']">
          <template slot="ex-before">
            <a-select
              v-model="action_type"
              placeholder="请选择操作类型"
              style="width: 225px; margin-right: 8px"
              :allowClear="true"
            >
              <a-select-option v-for="item in actionTypeList" :key="item.value" :value="item.value">
                {{ item.label }}
              </a-select-option>
            </a-select>

            <a-input-search
              placeholder="请输入任务编号"
              v-model="search_task_code"
              style="width: 224px; margin-right: 8px"
              @search="onSearch"
              allowClear
            />

            <a-input-search
              placeholder="请输入托盘码"
              v-model="search_loader_code"
              style="width: 224px; margin-right: 8px"
              @search="onSearch"
              allowClear
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
          <template slot="task_status" slot-scope="record">
            <!-- 任务状态;等待、运行中、结束、异常中止 -->
            <a-tag v-if="record === '等待'" color="red">等待</a-tag>
            <a-tag v-if="record === '运行中'" color="orange">运行中</a-tag>
            <a-tag v-if="record === '结束'" color="green">结束</a-tag>
            <a-tag v-if="record === '异常中止'" color="pink">异常中止</a-tag>
          </template>

          <template slot="begin_object" slot-scope="record">
            <div v-if="record.task_type == '出库'">
              {{
                `${record.begin_shelves_code}货架${record.begin_shelves_position_row}-${record.begin_shelves_position_col}货位(${record.begin_shelves_position_point_x},${record.begin_shelves_position_point_y},${record.begin_shelves_position_point_z})`
              }}
            </div>
            <div v-else-if="record.task_type == '入库'">
              {{ `${record.begin_equipment_name}` }}
            </div>
            <div v-else>-</div>
          </template>

          <template slot="target_object" slot-scope="record">
            <div v-if="record.task_type == '出库'">
              {{ `${record.end_equipment_name}` }}
            </div>
            <div v-else-if="record.task_type == '入库'">
              {{
                `${record.target_shelves_code}货架${record.target_shelves_position_row}-${record.target_shelves_position_col}货位(${record.target_shelves_position_point_x},${record.target_shelves_position_point_y},${record.target_shelves_position_point_z})`
              }}
            </div>
            <div v-else>-</div>
          </template>
          <template slot="action" slot-scope="record">
            <a-space>
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
  </page-header-wrapper>
</template>

<script>
import { PageHeaderWrapper } from '@/components/ProLayout'
import TableTools from '@/components/TableTools'
import { getPageList, deleteData } from '@/api/wcs/task'

export default {
  name: 'Events',
  components: {
    PageHeaderWrapper,
    TableTools
  },
  data() {
    return {
      loading: false,
      tablePagination: {
        // pageSizeOptions: ['100', '200'],
        // showSizeChanger: true,
        defaultPageSize: 100,
        showTotal: (total, range) => {
          return `第${range[0]}-${range[1]}条，总共 ${total} 条数据`
        },
        total: 0
      },
      pageInfo: {
        PageNum: 1,
        PageSize: 100
      },
      action_type: undefined,
      search_task_code: '',
      search_loader_code: '',
      actionTypeList: [
        { label: '入库', value: '002cfc70-89a2-11ed-a5d9-00163e10b48b' },
        { label: '出库', value: 'ba45546e-915b-11ed-a5d9-00163e10b48b' }
      ],
      columns: [
        {
          title: '任务编号',
          dataIndex: 'task_code'
        },
        {
          title: '任务类型',
          dataIndex: 'task_type'
        },
        {
          title: '托盘码',
          dataIndex: 'loader_code'
        },
        {
          title: '起始设备',
          scopedSlots: { customRender: 'begin_object' }
        },
        {
          title: '执行设备',
          dataIndex: 'process_equipment_name'
        },
        {
          title: '目标设备',
          scopedSlots: { customRender: 'target_object' }
        },
        // {
        //   title: '货架编号',
        //   dataIndex: 'shelves_code'
        // },
        // {
        //   title: '源货位（x,y,z）',
        //   scopedSlots: { customRender: 'begin_shelves_position' }
        // },
        // {
        //   title: '目标货位（x,y,z）',
        //   scopedSlots: { customRender: 'target_shelves_position' }
        // },
        {
          title: '状态',
          dataIndex: 'task_status',
          scopedSlots: { customRender: 'task_status' }
        },
        {
          title: '创建时间',
          dataIndex: 'createTime'
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
      this.pageInfo.PageSize = 100
      this.getData()
    },
    getData() {
      this.loading = true
      getPageList({
        ...this.pageInfo,
        task_type: this.action_type,
        task_code: this.search_task_code,
        loader_code: this.search_loader_code
      }).then((res) => {
        if (res.code === 200) {
          this.data = res.data.data
          this.tablePagination.total = res.data.total
        }

        this.loading = false
      })
    },
    onRowEdit(record, type) {
      console.log(`edit record:${type}`, record)
      if (type === 'delete') {
        this.loading = true
        deleteData({ id: record.id }).then((res) => {
          if (res.code === 200) {
            this.$message.success('操作成功')
            this.loading = false
            this.getData()
          }
        })
      }
    }
  }
}
</script>

<style lang="less" scoped></style>
