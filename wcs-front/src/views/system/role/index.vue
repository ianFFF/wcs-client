<template>
  <page-header-wrapper>
    <template v-slot:content>
      <!-- 条件搜索 -->
      <table-tools :actions="['search']" @onSearch="onSearch">
        <template slot="ex-after">
          <a-button class="btn" type="primary" @click="$refs.createForm.handleAdd()" v-hasPermi="['system:role:add']">
            新增
          </a-button>
          <a-button
            type="primary"
            class="btn"
            :disabled="single"
            @click="$refs.createForm.handleUpdate(undefined, ids)"
            v-hasPermi="['system:role:edit']"
          >
            修改
          </a-button>
          <a-button
            class="btn"
            type="danger"
            :disabled="multiple"
            @click="handleDelete"
            v-hasPermi="['system:role:remove']"
          >
            删除
          </a-button>
          <!-- <a-button type="primary" @click="handleExport" v-hasPermi="['system:role:export']">
            <a-icon type="download" />导出
          </a-button> -->
        </template>
      </table-tools>

      <!-- 增加修改 -->
      <create-form ref="createForm" :statusOptions="dict.type['sys_normal_disable']" @ok="getList" />

      <!-- 分配角色数据权限对话框 -->
      <create-data-scope-form ref="createDataScopeForm" @ok="getList" />
      <!-- 数据展示 -->
      <a-table
        class="main-table"
        :loading="loading"
        :size="tableSize"
        rowKey="roleId"
        :columns="columns"
        :data-source="list"
        :row-selection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
        :pagination="false"
        :bordered="tableBordered"
      >
        <span slot="status" slot-scope="text, record">
          <a-popconfirm
            ok-text="是"
            cancel-text="否"
            @confirm="confirmHandleStatus(record)"
            @cancel="cancelHandleStatus(record)"
          >
            <span slot="title">
              确认
              <b>{{ record.status === '1' ? '启用' : '停用' }}</b>
              {{ record.roleName }}
              的角色吗?
            </span>
            <a-switch checked-children="开" un-checked-children="关" :checked="record.status == 0" />
          </a-popconfirm>
        </span>
        <span slot="createTime" slot-scope="text, record">
          {{ parseTime(record.createTime) }}
        </span>
        <span slot="operation" slot-scope="text, record">
          <a @click="$refs.createForm.handleUpdate(record, undefined)" v-hasPermi="['system:role:edit']">
            <a-icon type="edit" />
            修改
          </a>
          <a-divider type="vertical" v-hasPermi="['system:role:remove']" />
          <a @click="handleDelete(record)" v-hasPermi="['system:role:remove']">
            <a-icon type="delete" />
            删除
          </a>
          <!-- <a-divider type="vertical" v-hasPermi="['system:role:edit']" />
          <a-dropdown v-hasPermi="['system:role:edit']">
            <a class="ant-dropdown-link" @click="(e) => e.preventDefault()">
              <a-icon type="double-right" />
              更多
            </a>
            <a-menu slot="overlay">
              <a-menu-item>
                <a @click="$refs.createDataScopeForm.handleDataScope(record)">
                  <a-icon type="lock" />
                  数据权限
                </a>
              </a-menu-item>
              <a-menu-item>
                <a @click="handleAuthUser(record)">
                  <a-icon type="user-add" />
                  分配用户
                </a>
              </a-menu-item>
            </a-menu>
          </a-dropdown> -->
        </span>
      </a-table>
      <!-- 分页 -->
      <a-pagination
        class="ant-table-pagination"
        show-size-changer
        show-quick-jumper
        :current="queryParam.pageNum"
        :total="total"
        :page-size="queryParam.pageSize"
        :showTotal="(total) => `共 ${total} 条`"
        @showSizeChange="onShowSizeChange"
        @change="changeSize"
      />
    </template>
  </page-header-wrapper>
</template>

<script>
import { listRole, delRole, changeRoleStatus } from '@/api/system/role'
import CreateForm from './modules/CreateForm'
import CreateDataScopeForm from './modules/CreateDataScopeForm'
import { tableMixin } from '@/store/table-mixin'
import TableTools from '@/components/TableTools'

export default {
  name: 'Role',
  components: {
    CreateForm,
    CreateDataScopeForm,
    TableTools
  },
  mixins: [tableMixin],
  dicts: ['sys_normal_disable'],
  data() {
    return {
      list: [],
      selectedRowKeys: [],
      selectedRows: [],
      // 高级搜索 展开/关闭
      advanced: false,
      // 非单个禁用
      single: true,
      // 非多个禁用
      multiple: true,
      ids: [],
      loading: false,
      total: 0,
      // 日期范围
      dateRange: [],
      queryParam: {
        pageNum: 1,
        pageSize: 10,
        roleName: undefined,
        roleKey: undefined,
        status: undefined
      },
      columns: [
        {
          title: '角色名',
          dataIndex: 'roleName',
          ellipsis: true,
          align: 'center'
        },
        {
          title: '角色编号',
          dataIndex: 'roleCode',
          align: 'center'
        },
        {
          title: '排序',
          dataIndex: 'orderNum',
          align: 'center'
        },
        {
          title: '状态',
          dataIndex: 'status',
          scopedSlots: { customRender: 'status' },
          align: 'center'
        },
        {
          title: '创建时间',
          dataIndex: 'createTime',
          scopedSlots: { customRender: 'createTime' },
          align: 'center'
        },
        {
          title: '操作',
          dataIndex: 'operation',
          width: '20%',
          scopedSlots: { customRender: 'operation' },
          align: 'center'
        }
      ]
    }
  },
  filters: {},
  created() {
    this.getList()
  },
  computed: {},
  watch: {},
  methods: {
    onSearch(val) {
      this.queryParam.roleName = val
      this.handleQuery()
    },
    /** 查询角色列表 */
    getList() {
      this.loading = true
      listRole(this.addDateRange(this.queryParam, this.dateRange)).then((response) => {
        this.list = response.data.data
        if (this.list) {
          this.list.map((item) => {
            item.status = item.isDeleted ? '1' : '0'
            item.roleId = item.id
          })
        }
        this.total = response.data.total
        this.loading = false
      })
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParam.pageNum = 1
      this.getList()
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.dateRange = []
      this.queryParam = {
        pageNum: 1,
        pageSize: 10,
        roleName: undefined,
        roleKey: undefined,
        status: undefined
      }
      this.handleQuery()
    },
    onShowSizeChange(current, pageSize) {
      this.queryParam.pageSize = pageSize
      this.getList()
    },
    changeSize(current, pageSize) {
      this.queryParam.pageNum = current
      this.queryParam.pageSize = pageSize
      this.getList()
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
      this.ids = this.selectedRows.map((item) => item.roleId)
      this.single = selectedRowKeys.length !== 1
      this.multiple = !selectedRowKeys.length
    },
    toggleAdvanced() {
      this.advanced = !this.advanced
    },
    /* 角色状态修改 */
    confirmHandleStatus(row) {
      const text = row.status === '1' ? '启用' : '停用'
      row.status = row.status === '0' ? '1' : '0'
      changeRoleStatus(row.roleId, row.status)
        .then(() => {
          this.$message.success(text + '成功', 3)

          this.getList()
        })
        .catch(function () {
          this.$message.error(text + '异常', 3)
        })
    },
    cancelHandleStatus(row) {},
    /** 删除按钮操作 */
    handleDelete(row) {
      var that = this
      const roleIds = row.roleId || this.ids

      this.$confirm({
        title: '确认删除所选中数据?',
        content: '当前选中编号为' + roleIds + '的数据',
        onOk() {
          return delRole(roleIds).then(() => {
            that.onSelectChange([], [])
            that.getList()
            that.$message.success('删除成功', 3)
          })
        },
        onCancel() {}
      })
    },
    /** 导出按钮操作 */
    handleExport() {
      var that = this
      this.$confirm({
        title: '是否确认导出?',
        content: '此操作将导出当前条件下所有数据而非选中数据',
        onOk() {
          that.download(
            'system/role/export',
            {
              ...that.queryParam
            },
            `role_${new Date().getTime()}.xlsx`
          )
        },
        onCancel() {}
      })
    },
    /** 分配用户操作 */
    handleAuthUser(row) {
      const roleId = row.roleId
      this.$router.push({ path: '/system/role/authUser', query: { roleId: roleId } })
    }
  }
}
</script>
