<template>
  <div>
    <a-modal v-model="visible" :title="title" @ok="handleOk" :maskClosable="false">
      <template slot="footer">
        <a-button key="cancel" @click="handleOk('cancel')">取消</a-button>
        <a-button key="submit" type="primary" :loading="loading" @click="handleOk('submit')">确定</a-button>
      </template>
      <a-spin tip="Loading..." :spinning="loading">
        <a-form-model
          ref="ruleForm"
          :model="form"
          :rules="rules"
          :label-col="labelCol"
          :wrapper-col="wrapperCol"
          :loading="true"
        >
          <a-form-model-item label="日志设备" prop="equipment_id">
            <a-select v-model="form.equipment_id" placeholder="请选择日志设备">
              <a-select-option v-for="item in equipmentList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="日志周期" prop="logs_interval">
            <a-input v-model="form.logs_interval" placeholder="请填写日志周期" type="number">
              <a-select slot="addonAfter" default-value="0" style="width: 80px" v-model="form.logs_interval_type">
                <a-select-option value="0">天</a-select-option>
                <a-select-option value="1">月</a-select-option>
              </a-select>
            </a-input>
          </a-form-model-item>

          <a-form-model-item label="是否启用" prop="logs_is_record">
            <a-switch v-model="form.logs_is_record" checked-children="开" un-checked-children="关" />
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </a-modal>
  </div>
</template>
<script>
import { getAllEquipment } from '@/api/wcs/equipment'
import { update, getDetailById } from '@/api/wcs/equipment_logs'

export default {
  name: 'EquipmentLogsForm',

  data() {
    return {
      visible: false,
      loading: false,
      title: '设备日志录入',

      labelCol: { span: 6 },
      wrapperCol: { span: 16 },
      form: {
        id: '',
        equipment_id: undefined,
        logs_interval: '',
        logs_interval_type: '1',
        logs_is_record: false
      },
      rules: {
        equipment_id: [{ required: true, message: '请选择设备' }]
      },
      equipmentList: []
    }
  },
  methods: {
    showModal(id) {
      console.log('showModal' + id)
      this.title = id ? '设备日志编辑' : '设备日志录入'
      this.form.id = id
      this.visible = true
      if (this.$refs.ruleForm) this.$refs.ruleForm.resetFields()
      this.getDefaultData()
    },
    handleOk(e) {
      if (e === 'cancel') {
        this.visible = false
        return
      }

      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          this.loading = true
          update({
            ...this.form
          }).then((res) => {
            if (res.code === 200) {
              this.visible = false
              this.loading = false
              this.$message.success('操作成功')
              this.$emit('onFinish', { type: e, data: this.form })
            }
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    getDefaultData() {
      getAllEquipment({ type: '' }).then((res) => {
        if (res.code === 200) {
          this.equipmentList = res.data.equipments.map((item) => {
            return {
              value: item.id,
              label: item.equipment_name
            }
          })
        }
      })

      if (!this.form.id) return
      this.loading = true

      getDetailById({ id: this.form.id }).then((res) => {
        if (res.code === 200) {
          this.form = { ...res.data }
          this.form.logs_interval_type = this.form.logs_interval_type.toString()
        }
        this.loading = false
      })
    }
  }
}
</script>
