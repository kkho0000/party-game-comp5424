## XR Group Project

<h3>操作说明：</h3>

<p>传送(A)：第一次按下A时，飞船进入传送准备状态，同时显示飞船虚像以告知玩家传送目的地；
        在传送准备状态按下A将触发传送行为，飞船被加速传送至目的地；
        在传送准备状态按下B则解除传送准备状态。
        PS：传送穿越实体障碍物是不被允许的。
        PPS：A键和B键在右手手柄上</p>

<p>复位(X)：按下空格飞船将进行自动复位；当飞船发生碰撞并产生不可控自旋时，
        使用此功能允许玩家在碰撞后快速调整飞船。其中X键在左手手柄上。</p>

<p>观察(Y)：按下Y玩家将进入观察状态，此时视角转动不会影响飞船行驶方向；
        在观察状态再次按下Y则返回正常行驶状态。其中Y键在左手手柄上。</p>

<h3>需要优化的部分功能</h3>	
<ol>
<li>主界面UI实现单机和联机的选择</li>
<li>联机时的飞机名字随机生成</li>
<li>实现新手教程（必须）</li>
<li>添加音效（必须）和主界面调整音量大小</li>
<li>手柄换皮成小熊手手（可选）</li>
<li>飞船速度手动调整</li>
<li>轨道箭头方向</li>
<li>SampleScene场景的Hierarchy结构可能需要重新部署一下</li>
<li>开场倒计时换成高清的</li>
</ol>
### 重构后的脚本：功能以及挂载

| 脚本名称               | 实现功能                                                     | 挂载物体                     |
| ---------------------- | ------------------------------------------------------------ | ---------------------------- |
| Checkpoint Manager     | 实现飞机与检查点的交互；传递信息到本飞船的HUD实现游戏进程的可视化；传递信息到Match Manager实现飞船Rank的更新 | Game Map - CheckPoint        |
| Collision Manager      | 实现飞机碰撞的检测，传递信息到碰撞脚本进行相关UI和功能的实时更新 | Space Craft                  |
| Match Manager          | 控制整体游戏进程                                             | Match Manager                |
| Orb Manager            | 实现能量球的逻辑，与Collision Manager和Orb UI进行通信        | Space Craft                  |
| Orb UI                 | 接收Orb Manager传来的信息并实时更新能量块的UI                | Space Craft - Canvas Console |
| Space Craft Controller | 实现飞船的基础移动                                           | Space Craft                  |
|                        |                                                              |                              |
|                        |                                                              |                              |

