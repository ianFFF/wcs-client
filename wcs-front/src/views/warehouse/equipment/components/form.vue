<template>
  <div>
    <a-modal v-model="visible" :title="title" @ok="handleOk" :maskClosable="false">
      <template slot="footer">
        <a-button key="cancel" @click="handleOk('cancel')">取消</a-button>
        <a-button key="test" :loading="loading" @click="handleOk('connectTest')" disabled>测试联通性</a-button>
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
          <a-form-model-item label="设备类型" prop="equipment_type">
            <a-select v-model="form.equipment_type" placeholder="请选择设备类型">
              <a-select-option v-for="item in equipmentTypeList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="设备名称" prop="equipment_name">
            <a-input v-model="form.equipment_name" placeholder="请填写设备名称" />
          </a-form-model-item>
          <a-form-model-item label="供应商" prop="equipment_supplier">
            <a-input v-model="form.equipment_supplier" placeholder="请填写设备供应商" />
          </a-form-model-item>
          <a-form-model-item label="IP" prop="ip">
            <a-input v-model="form.ip" placeholder="请填写设备网络IP" />
          </a-form-model-item>
          <a-form-model-item label="端口" prop="port">
            <a-input v-model="form.port" placeholder="请填写设备网络端口" type="number" />
          </a-form-model-item>

          <a-form-model-item label="输送线IO监听值" prop="io_point">
            <a-input v-model="form.io_point" placeholder="输送线IO监听值" />
          </a-form-model-item>

          <a-form-model-item label="起始x值" prop="point_x">
            <a-input v-model="form.point_x" placeholder="起始x值" type="number" addon-after="mm" />
          </a-form-model-item>

          <a-form-model-item label="起始z值" prop="point_z">
            <a-input v-model="form.point_z" placeholder="起始z值" type="number" addon-after="mm" />
          </a-form-model-item>

          <a-form-model-item label="关联设备" prop="relation_object_id">
            <a-select v-model="form.relation_object_id" placeholder="请选择设备">
              <a-select-option v-for="item in equipmentList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="库区" prop="warehouse_area">
            <a-select v-model="form.warehouse_area" placeholder="请选择设备所属库区">
              <a-select-option v-for="item in warehouseAreaList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="上游系统ID" prop="upper_system_id">
            <a-input v-model="form.upper_system_id" placeholder="上游系统ID" disabled />
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </a-modal>
  </div>
</template>
<script>
import { getOptions, update, getDetailById, getAllEquipment } from '@/api/wcs/equipment'

export default {
  name: 'EquipmentForm',

  data() {
    return {
      visible: false,
      loading: false,
      title: '设备输入',

      labelCol: { span: 6 },
      wrapperCol: { span: 16 },
      form: {
        id: '',
        equipment_type: undefined,
        equipment_name: '',
        equipment_supplier: '',
        upper_system_id: '',
        ip: '',
        port: '',
        warehouse_area: undefined,
        relation_object_id: undefined,
        point_x: '',
        point_y: '',
        point_z: '',
        io_point: ''
      },
      rules: {
        equipment_type: [{ required: true, message: '请选择设备类型' }],
        equipment_name: [{ required: true, message: '请填写设备名称' }]
      },
      equipmentTypeList: [],
      equipmentList: [],
      warehouseAreaList: []
    }
  },
  methods: {
    showModal(id) {
      console.log('showModal' + id)
      this.title = id ? '设备编辑' : '设备输入'
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
      if (e === 'connectTest') {
        this.loading = true
        setTimeout(() => {
          this.loading = false
        }, 3000)
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
      getOptions().then((res) => {
        if (res.code === 200) {
          this.equipmentTypeList = res.data.equipmentTypes.map((item) => {
            return {
              value: item.id,
              label: item.equipment_type_name
            }
          })

          this.warehouseAreaList = res.data.wareHouse.map((item) => {
            return {
              value: item.id,
              label: item.warehouse_name
            }
          })
        }
      })

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
        }
        this.loading = false
      })
    }
  }
}
</script>
