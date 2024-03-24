<template>
  <page-header-wrapper>
    <template v-slot:content>
      <!-- 条件搜索 -->
      <table-tools :actions="['search']" @onSearch="onSearch">
        <template slot="ex-after">
          <a-button class="btn" type="primary" @click="$refs.createForm.handleAdd()" v-hasPermi="['system:user:add']">
            新增
          </a-button>
          <a-button
            class="btn"
            type="primary"
            :disabled="single"
            @click="$refs.createForm.handleUpdate(undefined, ids)"
            v-hasPermi="['system:user:edit']"
          >
            修改
          </a-button>
          <a-button
            class="btn"
            type="danger"
            :disabled="multiple"
            @click="handleDelete"
            v-hasPermi="['system:user:remove']"
          >
            删除
          </a-button>
          <!-- <a-button
            type="dashed"
            @click="$refs.importExcel.importExcelHandleOpen()"
            v-hasPermi="['system:user:import']"
          >
            <a-icon type="import" />导入
          </a-button> -->
          <!-- <a-button type="primary" @click="handleExport" v-hasPermi="['system:user:export']">
            <a-icon type="download" />导出
          </a-button> -->
        </template>
      </table-tools>

      <!-- 增加修改 -->
      <!-- 创建/编辑用户,单独封装了组件 -->
      <create-form
        ref="createForm"
        :deptOptions="deptOptions"
        :statusOptions="dict.type['sys_normal_disable']"
        :sexOptions="dict.type['sys_user_sex']"
        @ok="getList"
      />
      <!-- 修改密码抽屉 -->
      <reset-password ref="resetPassword" />
      <!-- 分配角色模态框 -->
      <auth-role ref="authRole" />
      <!-- 上传文件 -->
      <import-excel ref="importExcel" @ok="getList" />
      <!-- 数据展示 -->
      <a-table
        class="main-table"
        :loading="loading"
        :size="tableSize"
        rowKey="userId"
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
              {{ record.nickName }}的用户吗?
            </span>
            <a-switch checked-children="开" un-checked-children="关" :checked="record.status == 0" />
          </a-popconfirm>
        </span>
        <span slot="createTime" slot-scope="text, record">
          {{ parseTime(record.createTime) }}
        </span>
        <span slot="operation" slot-scope="text, record" v-if="record.userId !== 1">
          <a @click="$refs.createForm.handleUpdate(record, undefined)" v-hasPermi="['system:user:edit']">
            <a-icon type="edit" />
            修改
          </a>
          <a-divider type="vertical" v-hasPermi="['system:user:remove']" />
          <a @click="handleDelete(record)" v-hasPermi="['system:user:remove']">
            <a-icon type="delete" />
            删除
          </a>
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
import { listUser, delUser, changeUserStatus, deptTreeSelect } from '@/api/system/user'
import AuthRole from './modules/AuthRole'
import ResetPassword from './modules/ResetPassword'
import CreateForm from './modules/CreateForm'
import ImportExcel from './modules/ImportExcel'
import DeptTree from './modules/DeptTree'
import { tableMixin } from '@/store/table-mixin'
import TableTools from '@/components/TableTools'

export default {
  name: 'User',
  components: {
    AuthRole,
    ResetPassword,
    CreateForm,
    ImportExcel,
    DeptTree,
    TableTools
  },
  mixins: [tableMixin],
  dicts: ['sys_normal_disable', 'sys_user_sex'],
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
      // 部门树选项
      deptOptions: [
        {
          id: 0,
          label: '',
          children: []
        }
      ],
      // 日期范围
      dateRange: [],
      queryParam: {
        pageNum: 1,
        pageSize: 10,
        userName: undefined,
        phonenumber: undefined,
        status: undefined,
        deptId: undefined
      },
      columns: [
        {
          title: '用户编号',
          dataIndex: 'userId',
          align: 'center'
        },
        {
          title: '用户名',
          dataIndex: 'userName',
          align: 'center'
        },
        {
          title: '用户昵称',
          dataIndex: 'nick',
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
          align: 'center'
        },
        {
          title: '操作',
          dataIndex: 'operation',
          scopedSlots: { customRender: 'operation' },
          align: 'center'
        }
      ]
    }
  },
  filters: {},
  created() {
    this.getList()
    this.getDeptTree()
  },
  computed: {},
  watch: {},
  methods: {
    onSearch(val) {
      this.queryParam.userName = val
      this.handleQuery()
    },
    /** 查询用户列表 */
    getList() {
      this.loading = true
      listUser(this.addDateRange(this.queryParam, this.dateRange)).then((response) => {
        this.list = response.data.data
        if (this.list) {
          this.list.map((item) => {
            item.status = item.isDeleted ? '1' : '0'
            item.userId = item.id
          })
        }

        this.total = response.data.total
        this.loading = false
      })
    },
    /** 查询部门下拉树结构 */
    getDeptTree() {
      deptTreeSelect().then((response) => {
        this.deptOptions = response.data
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
        userName: undefined,
        phonenumber: undefined,
        status: undefined,
        deptId: undefined
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
      this.ids = this.selectedRows.map((item) => item.userId)
      this.single = selectedRowKeys.length !== 1
      this.multiple = !selectedRowKeys.length
    },
    toggleAdvanced() {
      this.advanced = !this.advanced
    },
    clickDeptNode(deptId) {
      this.queryParam.deptId = deptId
      this.handleQuery()
    },
    /* 用户状态修改 */
    confirmHandleStatus(row) {
      const text = row.status === '1' ? '启用' : '关闭'
      row.status = row.status === '0' ? '1' : '0'
      changeUserStatus(row.userId, row.status)
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
      const userIds = row.userId || this.ids
      this.$confirm({
        title: '确认删除所选中数据?',
        content: '当前选中编号为' + userIds + '的数据',
        onOk() {
          return delUser(userIds).then(() => {
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
            'system/user/export',
            {
              ...that.queryParam
            },
            `user_${new Date().getTime()}.xlsx`
          )
        },
        onCancel() {}
      })
    }
  }
}
</script>
