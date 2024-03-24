<template>
  <div>
    <a-modal v-model="visible" :title="title" @ok="handleOk">
      <template slot="footer">
        <a-button key="cancel" @click="handleOk('cancel')">取消</a-button>
        <a-button key="submit" type="primary" :loading="loading" @click="handleOk('submit')">确定</a-button>
      </template>
      <a-spin tip="Loading..." :spinning="loading">
        <a-form-model ref="ruleForm" :model="form" :rules="rules" :label-col="labelCol" :wrapper-col="wrapperCol">
          <a-form-model-item label="操作类型" prop="action_type">
            <a-select v-model="form.action_type" placeholder="请选择">
              <a-select-option v-for="item in lockedType" :key="item.value" :value="item.value">
                {{ item.label }}
              </a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="货架" prop="shelves_id">
            <a-select v-model="form.shelves_id" placeholder="请选货架">
              <a-select-option v-for="item in shelvesList" :key="item.value" :value="item.value">
                {{ item.label }}
              </a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="货位" prop="shelves_position_name">
            <a-input
              class="select-hw"
              v-model="form.shelves_position_name"
              placeholder="请选择货位"
              @click="onSelectHw"
              addon-after="(x,y,z)"
              :disabled="!this.form.shelves_id"
            />
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </a-modal>

    <shelves-position ref="ShelvesPosition" :type="'lock'" @onFinish="onFinishSelectHw" />
  </div>
</template>
<script>
import { selctListForManual, updateShelvesPosition } from '@/api/wcs/shelves'
import { updateConsoleShelvesPosition } from '@/api/wcs/task'

import ShelvesPosition from '@/components/ShelvesPosition'

export default {
  name: 'PositionForm',
  components: { ShelvesPosition },
  data() {
    return {
      visible: false,
      loading: false,
      title: '货位锁定',

      labelCol: { span: 6 },
      wrapperCol: { span: 16 },
      form: {
        action_type: 1,
        shelves_id: undefined,
        shelves_position_id: '',
        shelves_position_name: '',
        equipment_id: undefined
      },
      rules: {
        shelves_id: [{ required: true, message: '请选择货架' }],
        shelves_position_name: [{ required: true, message: '请选择货位' }]
      },
      lockedType: [
        { value: 1, label: '冻结' },
        { value: 2, label: '异常' },
        { value: 0, label: '取消冻结、异常' }
      ],
      shelvesList: []
    }
  },
  methods: {
    showModal() {
      this.visible = true
      if (this.$refs.ruleForm) this.$refs.ruleForm.resetFields()
      this.getShelves('')
    },
    handleOk(e) {
      if (e === 'cancel') {
        this.visible = false
        return
      }

      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          this.loading = true
          updateShelvesPosition({
            id: this.form.shelves_position_id,
            status: this.form.action_type
          }).then((res) => {
            this.loading = false
            if (res.code === 200 && res.status) {
              this.$message.success('操作成功')
              this.visible = false

              updateConsoleShelvesPosition({ hwId: this.form.shelves_position_id, status: this.form.action_type })
            } else {
              this.$message.error(res.message)
            }
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    getShelves(equipmentId) {
      selctListForManual().then((res) => {
        if (res.code === 200) {
          this.shelvesList = res.data.shelves.map((item) => {
            return {
              value: item.id,
              label: item.shelves_code
            }
          })
        }
      })
    },
    onSelectHw() {
      if (this.form.shelves_id) {
        this.$refs.ShelvesPosition.showModal(this.form.shelves_id)
      }
    },
    onFinishSelectHw(value) {
      if (value.data) {
        this.form.shelves_position_name = `${value.data.row}-${value.data.col}货位（${value.data.point_x},${value.data.point_y},${value.data.point_z}）`
        this.form.shelves_position_id = value.data.id
      }
    }
  }
}
</script>
<style lang="less">
.select-hw {
  .ant-input {
    cursor: pointer;
  }
}
</style>
