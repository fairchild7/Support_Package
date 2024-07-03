---------Readme - UI Manager------------------------------------

- các UI cần kế thừa UICanvas, sau đó tạo prefab và để trong Resources/UI
- Không cần để UI trên hierarchy
- Cần 1 object cầm script UIManager, kéo canvas root/ hoặc parent chung của các UI
vào CanvasParentTF
- Bật/ tắt các UI qua UIManager:

Ví dụ: UITest : UICanvas
	
	UIManager.Ins.OpenUI<UITest>();	
	//nếu UI chưa đc load thì instantiate, sau đó set active true
	//sẽ gọi đến function Open() của UICanvas

	UIManager.Ins.GetUI<UITest>(); 
	//nếu UI chưa đc load thì instantiate, giữ nguyên trạng thái active của prefab

	UIManager.Ins.CloseUI<UITest>(); 
	//set active false UITest, không destroy
	//sẽ gọi đến function Close() của UICanvas	

- Open/Get/Close UI đều trả về instance của UI được gọi, có thể qua đó call các function
	UIManager.Ins.GetUI<UITest>().PrepareUI();