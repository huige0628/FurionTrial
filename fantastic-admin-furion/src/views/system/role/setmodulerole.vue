<template>
    <el-dialog
        :visible.sync="show"
        :close-on-click-modal="false"
        title="分配菜单权限"
        width="50%"
        top="10vh"
        scrollable
    >
        <div style="max-height: 550px; overflow-x: auto;" class="form-container">
            <el-tree
                :data="treeData"
                show-checkbox
                node-key="id"
                default-expand-all
                :expand-on-click-node="false"
                :default-checked-keys="defaultCheckedKey"
                @check-change="checkChange"
            >
                <div slot-scope="{ data }" class="action-group">
                    <div
                        class="action-text"
                        :style="{ width: (4 - data.lv) * 18 + 150 + 'px' }"
                    >
                        {{ data.label }}
                    </div>
                    <div class="action-item">
                        <el-checkbox
                            v-for="item in data.action"
                            :key="item.elementId"
                            v-model="item.checked"
                        >
                            {{ item.elementName }}
                        </el-checkbox>
                    </div>
                </div>
            </el-tree>
        </div>
        <div slot="footer" class="dialog-footer">
            <el-button @click="close">取 消</el-button>
            <el-button type="primary" @click="ok">确 定</el-button>
        </div>
    </el-dialog>
</template>
<script>
/**
 * 用户添加编辑
 */
export default {
    components: {},
    data() {
        return {
            show: false,
            loading: false,
            roleId: null,
            treeData: [],
            modulePermissions: [],
            defaultCheckedKey: [] // 默认选中的模块
        }
    },
    mounted() {},
    methods: {
        loadModuleRoleTree() {
            this.$api
                .get('api/system/getRoleModuleTree', {
                    params: {
                        roleId: this.roleId
                    }
                })
                .then(res => {
                    this.treeData = res.data
                })
        },
        loadRoleModule() {
            this.defaultCheckedKey = []
            this.$api
                .get('api/system/getrolemodule', {
                    params: {
                        roleId: this.roleId
                    }
                })
                .then(res => {
                    if (res.data) {
                        this.defaultCheckedKey = res.data
                    }
                })
        },
        // 菜单点击回调事件
        checkChange(node, selected) {
            node.action.forEach(x => {
                this.$set(x, 'checked', selected)
            })
        },
        getChildAction(data) {
            data.forEach(x => {
                let checkedPermission = x.action.filter(f => {
                    return f.checked
                })
                if (checkedPermission.length > 0) {
                    let actions = checkedPermission.map(m => {
                        return { elementName: m.elementName, elementId: m.elementId }
                    })
                    this.modulePermissions.push({
                        moduleId: x.id,
                        action: actions
                    })
                }
                if (x.children) {
                    this.getChildAction(x.children)
                }
            })
        },
        // 确定
        ok: function() {
            this.loading = true
            this.modulePermissions = []
            this.treeData.forEach(x => {
                let checkedPermission = x.action.filter(f => {
                    return f.checked
                })
                if (checkedPermission.length > 0) {
                    let actions = checkedPermission.map(m => {
                        return { elementName: m.elementName, elementId: m.elementId }
                    })
                    this.modulePermissions.push({
                        moduleId: x.id,
                        action: actions
                    })
                }
                if (x.children) {
                    this.getChildAction(x.children)
                }
            })
            let _data = {
                roleId: this.roleId,
                modulePermission: this.modulePermissions
            }
            this.$api
                .post('api/system/setmoduleroles', _data)
                .then(res => {
                    if (res.succeeded) {
                        this.$message.success('保存成功')
                        this.$emit('on-ok') // 成功响应事件
                        this.close()
                    } else {
                        this.$message.error(res.errors)
                    }
                })
                .catch(() => (this.loading = false))
        },
        // 关闭窗口
        close: function() {
            this.loading = false
            this.show = false
        },
        // 打开窗口
        open: function(row) {
            this.show = true
            this.roleId = row.roleId
            this.loadRoleModule() // 获取当前角色拥有的模块权限
            this.loadModuleRoleTree() // 加载模块树
        }
    }
}
</script>
<style>
.t-tree {
    display: flex;

    /* padding: 100px; */
}
.action-group {
    width: 100%;
    display: flex;
}
.action-text {
    /* width: 200px; */
    margin-right: 10px;
}
.action-item {
    flex: 1;
}
.action-item > label {
    width: 70px;
    margin-left: 10px;
}
</style>
