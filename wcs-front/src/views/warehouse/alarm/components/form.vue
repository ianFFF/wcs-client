<template>
  <div>
    <a-modal v-model="visible" :title="title" @ok="handleOk">
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
          <a-form-model-item label="设备类型" prop="equipment_type_id">
            <a-select v-model="form.equipment_type_id" placeholder="请选择设备类型">
              <a-select-option v-for="item in equipmentTypeList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="设备名称" prop="equipment_id">
            <a-select v-model="form.equipment_id" placeholder="请选择设备">
              <a-select-option v-for="item in equipmentList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="异常等级" prop="alarm_level">
            <a-select v-model="form.alarm_level" placeholder="请选择异常等级">
              <a-select-option v-for="item in alarmLevelList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="异常编码" prop="alarm_code">
            <a-input v-model="form.alarm_code" placeholder="请填写异常编码" />
          </a-form-model-item>
          <a-form-model-item label="异常说明" prop="alarm_remark">
            <a-textarea v-model="form.alarm_remark" placeholder="请填写异常说明" :rows="4" />
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </a-modal>
  </div>
</template>
<script>
import { getOptions, getAllEquipment } from '@/api/wcs/equipment'
import { getAllAlarm, update, getDetailById } from '@/api/wcs/equipment_alarm'

export default {
  name: 'EquipmentAlarmForm',

  data() {
    return {
      visible: false,
      loading: false,
      title: '异常类型输入',

      labelCol: { span: 6 },
      wrapperCol: { span: 16 },
      form: {
        id: '',
        equipment_type_id: undefined,
        equipment_id: undefined,
        alarm_level: undefined,
        alarm_code: '',
        alarm_remark: ''
      },
      rules: {
        equipment_type_id: [{ required: true, message: '请选择设备类型' }],
        equipment_id: [{ required: true, message: '请选择设备' }],
        alarm_level: [{ required: true, message: '请选择异常等级' }],
        alarm_code: [{ required: true, message: '请填写异常编码' }],
        alarm_remark: [{ required: true, message: '请填写异常说明' }]
      },
      equipmentTypeList: [],
      equipmentList: [],
      alarmLevelList: []
    }
  },
  watch: {
    'form.equipment_type_id': {
      handler(val) {
        if (val) {
          this.getEquipments(val)
        }
      },
      immediate: true
    }
  },
  methods: {
    showModal(id) {
      console.log('showModal' + id)
      this.title = id ? '异常类型编辑' : '异常类型输入'
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
    getEquipments(type) {
      getAllEquipment({ type }).then((res) => {
        if (res.code === 200) {
          this.equipmentList = res.data.equipments.map((item) => {
            return {
              value: item.id,
              label: item.equipment_name
            }
          })
        }
      })
    },
    getDefaultData() {
      getOptions().then((res) => {
        if (res.code === 200) {
          this.equipmentTypeList = res.data.equipmentTypes.map((item) => {
            return {
              value: item.id,
              label: item.equipment_type_name
            }
          })
        }
      })

      getAllAlarm().then((res) => {
        if (res.code === 200) {
          this.alarmLevelList = res.data.alarms.map((item) => {
            return {
              value: item.id,
              label: item.alarm_level
            }
          })
        }
      })

      if (!this.form.id) return
      this.loading = true
      getDetailById({ id: this.form.id }).then((res) => {
        if (res.code === 200) {
          this.form = { ...res.data }
        }
        this.loading = false
      })
    }
  }
}
</script>
