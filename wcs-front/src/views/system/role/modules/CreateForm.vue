<template>
  <a-drawer width="35%" :label-col="4" :wrapper-col="14" :visible="open" @close="onClose">
    <a-divider orientation="left">
      <b>{{ formTitle }}</b>
    </a-divider>
    <a-form-model ref="form" :model="form" :rules="rules">
      <a-form-model-item label="角色名称" prop="roleName">
        <a-input v-model="form.roleName" placeholder="请输入" />
      </a-form-model-item>
      <a-form-model-item prop="roleKey">
        <span slot="label">
          权限字符
          <a-tooltip>
            <template slot="title"> 控制器中定义的权限字符，如：@PreAuthorize(`@ss.hasRole('admin')`) </template>
            <a-icon type="question-circle-o" />
          </a-tooltip>
        </span>
        <a-input v-model="form.roleKey" placeholder="请输入" />
      </a-form-model-item>
      <a-form-model-item label="排序" prop="roleSort">
        <a-input-number placeholder="请输入" v-model="form.roleSort" :min="0" style="width: 100%" />
      </a-form-model-item>
      <a-form-model-item label="状态" prop="status">
        <a-radio-group v-model="form.status" button-style="solid">
          <a-radio-button v-for="(d, index) in statusOptions" :key="index" :value="d.value">{{
            d.label
          }}</a-radio-button>
        </a-radio-group>
      </a-form-model-item>
      <a-form-model-item label="菜单权限">
        <a-checkbox @change="handleCheckedTreeExpand($event)"> 展开/折叠 </a-checkbox>
        <a-checkbox @change="handleCheckedTreeNodeAll($event)"> 全选/全不选 </a-checkbox>
        <a-checkbox @change="handleCheckedTreeConnect($event)" :checked="form.menuCheckStrictly"> 父子联动 </a-checkbox>
        <a-tree
          v-model="menuCheckedKeys"
          checkable
          :checkStrictly="!form.menuCheckStrictly"
          :expanded-keys="menuExpandedKeys"
          :auto-expand-parent="autoExpandParent"
          :tree-data="menuOptions"
          @check="onCheck"
          @expand="onExpandMenu"
          :replaceFields="defaultProps"
        />
      </a-form-model-item>
      <a-form-model-item label="备注" prop="remark">
        <a-input v-model="form.remark" placeholder="请输入" type="textarea" allow-clear />
      </a-form-model-item>
      <div class="bottom-control">
        <a-space>
          <a-button type="primary" :loading="submitLoading" @click="submitForm"> 保存 </a-button>
          <a-button type="dashed" @click="cancel"> 取消 </a-button>
        </a-space>
      </div>
    </a-form-model>
  </a-drawer>
</template>

<script>
import { getRole, addRole, updateRole } from '@/api/system/role'
import { treeselect as menuTreeselect, roleMenuTreeselect } from '@/api/system/menu'

export default {
  name: 'CreateForm',
  props: {
    statusOptions: {
      type: Array,
      required: true
    }
  },
  components: {},
  data() {
    return {
      menuList: [
        '1598953116903018496',
        '1599237829202939904',
        '1628014473455472640',
        '1604359353652088832',
        '1605063837399257088'
      ],
      submitLoading: false,
      menuExpandedKeys: [],
      autoExpandParent: false,
      menuCheckedKeys: [],
      halfCheckedKeys: [],
      menuOptions: [],
      formTitle: '',
      // 表单参数
      form: {
        roleId: undefined,
        roleName: undefined,
        roleKey: undefined,
        roleSort: 0,
        status: '0',
        menuIds: [],
        menuCheckStrictly: true,
        remark: undefined
      },

      open: false,
      menuExpand: false,
      menuNodeAll: false,
      rules: {
        roleName: [{ required: true, message: '角色名称不能为空', trigger: 'blur' }],
        roleKey: [{ required: true, message: '权限字符不能为空', trigger: 'blur' }],
        roleSort: [{ required: true, message: '显示顺序不能为空', trigger: 'blur' }]
      },
      defaultProps: {
        children: 'children',
        title: 'label',
        key: 'id'
      }
    }
  },
  filters: {},
  created() {},
  computed: {},
  watch: {},
  methods: {
    onExpandMenu(expandedKeys) {
      this.menuExpandedKeys = expandedKeys
      this.autoExpandParent = false
    },
    /** 查询菜单树结构 */
    getMenuTreeselect() {
      menuTreeselect().then((response) => {
        var options = []

        if (response.data) {
          response.data.forEach((m) => {
            if (this.menuList.indexOf(m.id) >= 0 || this.menuList.indexOf(m.parentId) >= 0) {
              options.push({
                id: m.id,
                label: m.menuName,
                parentId: m.parentId,
                children: m.children
              })
            }
          })
        }

        var data = this.handleTree(options)

        this.menuOptions = data
      })
    },
    // 所有菜单节点数据
    getMenuAllCheckedKeys() {
      // 全选与半选
      return this.menuCheckedKeys.concat(this.halfCheckedKeys)
    },
    getAllMenuNode(nodes) {
      if (!nodes || nodes.length === 0) {
        return []
      }
      nodes.forEach((node) => {
        this.menuCheckedKeys.push(node.id)
        return this.getAllMenuNode(node.children)
      })
    },
    // 回显过滤
    selectNodefilter(nodes, parentIds) {
      if (!nodes || nodes.length === 0) {
        return
      }
      nodes.forEach((node) => {
        // 父子关联模式且当前元素有父级
        const currentIndex = this.menuCheckedKeys.indexOf(node.id)
        // 当前节点存在,且父节点不存在，则说明父节点应是半选中状态
        // parentIds没有数据的时候认为是顶级菜单，不用给半选中状态
        if (currentIndex > -1 && parentIds && parentIds.length > 0) {
          parentIds.forEach((parentId) => {
            if (this.halfCheckedKeys.indexOf(parentId) === -1) {
              this.halfCheckedKeys.push(parentId)
            }
          })
          parentIds = []
        }
        // 防重
        const isExist = this.halfCheckedKeys.indexOf(node.id)
        const isExistParentIds = parentIds.indexOf(node.id)
        const newParentIds = [...parentIds]
        if (isExist === -1 && isExistParentIds === -1 && currentIndex === -1) {
          newParentIds.push(node.id)
        }
        this.selectNodefilter(node.children, parentIds)
      })
    },
    handleCheckedTreeNodeAll(value) {
      if (value.target.checked) {
        this.getAllMenuNode(this.menuOptions)
      } else {
        this.menuCheckedKeys = []
        this.halfCheckedKeys = []
      }
    },
    handleCheckedTreeExpand(value) {
      if (value.target.checked) {
        const treeList = this.menuOptions
        for (let i = 0; i < treeList.length; i++) {
          this.menuExpandedKeys.push(treeList[i].id)
        }
      } else {
        this.menuExpandedKeys = []
      }
    },
    // 树权限（父子联动）
    handleCheckedTreeConnect(value) {
      this.form.menuCheckStrictly = !this.form.menuCheckStrictly
    },
    /** 根据角色ID查询菜单树结构 */
    getRoleMenuTreeselect(roleId) {
      return roleMenuTreeselect(roleId).then((response) => {
        return response
      })
    },
    onCheck(checkedKeys, info) {
      if (!this.form.menuCheckStrictly) {
        let currentCheckedKeys = []
        if (this.menuCheckedKeys.checked) {
          currentCheckedKeys = currentCheckedKeys.concat(this.menuCheckedKeys.checked)
        }
        if (this.menuCheckedKeys.halfChecked) {
          currentCheckedKeys = currentCheckedKeys.concat(this.menuCheckedKeys.halfChecked)
        }
        this.menuCheckedKeys = currentCheckedKeys
      } else {
        // 半选节点
        this.halfCheckedKeys = info.halfCheckedKeys
        this.menuCheckedKeys = checkedKeys
      }
    },
    onClose() {
      this.open = false
    },
    // 取消按钮
    cancel() {
      this.open = false
      this.reset()
    },
    // 表单重置
    reset() {
      if (this.$refs.menu !== undefined) {
        this.menuCheckedKeys = []
        this.halfCheckedKeys = []
      }
      this.menuExpand = false
      this.menuNodeAll = false
      this.menuExpandedKeys = []
      this.autoExpandParent = false
      this.menuCheckedKeys = []
      this.halfCheckedKeys = []
      this.form = {
        roleId: undefined,
        roleName: undefined,
        roleKey: undefined,
        roleSort: 0,
        status: 'false',
        menuIds: [],
        menuCheckStrictly: true,
        remark: undefined
      }
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset()
      this.getMenuTreeselect()
      this.open = true
      this.formTitle = '添加角色'
    },
    /** 修改按钮操作 */
    handleUpdate(row, ids) {
      this.reset()
      const roleId = row ? row.roleId : ids
      this.getMenuTreeselect()

      getRole(roleId).then((response) => {
        // this.form = response.data
        this.form.roleId = response.data.id
        this.form.roleName = response.data.roleName
        this.form.roleKey = response.data.roleCode
        this.form.roleSort = response.data.orderNum
        this.form.status = response.data.isDeleted + ''
        this.form.remark = response.data.remark
        this.open = true
        this.$nextTick(() => {
          roleMenuTreeselect(roleId).then((res) => {
            console.log(res)
            if (res && res.data) {
              res.data.forEach((item) => {
                this.menuCheckedKeys.push(item.id)
              })
            }
            // this.menuCheckedKeys = res.checkedKeys
            // 过滤回显时的半选中node(父)
            if (this.form.menuCheckStrictly) {
              this.selectNodefilter(this.menuOptions, [])
            }
          })
        })
        this.formTitle = '修改角色'
      })
    },
    /** 提交按钮 */
    submitForm: function () {
      this.$refs.form.validate((valid) => {
        if (valid) {
          this.submitLoading = true
          if (this.form.roleId !== undefined) {
            this.form.menuIds = this.getMenuAllCheckedKeys()
            updateRole({
              role: {
                id: this.form.roleId,
                roleName: this.form.roleName,
                roleCode: this.form.roleKey,
                orderNum: this.form.roleSort,
                IsDeleted: this.form.status,
                menuCheckStrictly: this.form.menuCheckStrictly,
                deptCheckStrictly: false,
                dataScope: 0
              },
              menuIds: this.form.menuIds,
              deptIds: [],
              menuCheckStrictly: true
            })
              .then((response) => {
                this.$message.success('修改成功', 3)
                this.open = false
                this.$emit('ok')
              })
              .finally(() => {
                this.submitLoading = false
              })
          } else {
            this.form.menuIds = this.getMenuAllCheckedKeys()

            addRole({
              role: {
                roleName: this.form.roleName,
                roleCode: this.form.roleKey,
                orderNum: this.form.roleSort,
                IsDeleted: this.form.status,
                menuCheckStrictly: this.form.menuCheckStrictly,
                deptCheckStrictly: false,
                dataScope: 0
              },
              menuIds: this.form.menuIds,
              deptIds: [],
              menuCheckStrictly: true
            })
              .then((response) => {
                this.$message.success('新增成功', 3)
                this.open = false
                this.$emit('ok')
              })
              .finally(() => {
                this.submitLoading = false
              })
          }
        } else {
          return false
        }
      })
    }
  }
}
</script>
