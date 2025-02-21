using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HudElements
{
    public class HealthBar : VisualElement, INotifyValueChanged<float>
    {
        public int width { get; set; }
        public int height { get; set; }

        public void SetValueWithoutNotify(float newValue)
        {
            m_value = newValue;
        }

        private float m_value;
        public float value
        {
            get
            {
                m_value = Mathf.Clamp(m_value, 0, 1);
                return m_value;
            }
            set
            {
                if (EqualityComparer<float>.Default.Equals(m_value, value)) return;
                if (this.panel != null)
                {
                    using (ChangeEvent<float> pooled = ChangeEvent<float>.GetPooled(this.m_value, value))
                    {
                        pooled.target = (IEventHandler)this;
                        this.SetValueWithoutNotify(value);
                        this.SendEvent((EventBase)pooled);

                    }
                }
                else
                {
                    SetValueWithoutNotify(value);
                }
            }
        }
        public enum FillType
        {
            Horizontal,
            Vertical
        }
        public Color fillColor { get; set; }   

        public FillType fillType;
        private VisualElement hbParent;
        private VisualElement hbBackGround;
        private VisualElement hbForeGround;
        public new class UxmlFactory : UxmlFactory<HealthBar, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits{

            UxmlIntAttributeDescription m_width = new UxmlIntAttributeDescription() { name = "width", defaultValue = 300 };
            UxmlIntAttributeDescription m_height=new UxmlIntAttributeDescription() { name = "height", defaultValue = 50 };
            UxmlFloatAttributeDescription m_value = new UxmlFloatAttributeDescription() { name = "value", defaultValue = 1 };
            UxmlEnumAttributeDescription<HealthBar.FillType> m_fillType = new UxmlEnumAttributeDescription<FillType>() { name = "fill-type", defaultValue = 0 };
            UxmlColorAttributeDescription m_fillColor = new UxmlColorAttributeDescription() { name = "fill-color", defaultValue = Color.red };
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as HealthBar;
                ate.width = m_width.GetValueFromBag(bag, cc);
                ate.height=m_height.GetValueFromBag(bag,cc);
                ate.value = m_value.GetValueFromBag(bag, cc);
                ate.fillType=m_fillType.GetValueFromBag(bag, cc);
                ate.fillColor=m_fillColor.GetValueFromBag(bag, cc);

                ate.Clear();
                VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("GameUI/healthbar");
                VisualElement healthbar = vt.Instantiate();
                ate.hbParent = healthbar.Q<VisualElement>("healthbar");
                ate.hbBackGround = healthbar.Q<VisualElement>("background");
                ate.hbForeGround = healthbar.Q<VisualElement>("foreground");
                ate.Add(healthbar);

                ate.hbParent.style.width = ate.width;
                ate.hbParent.style.height = ate.height;
                ate.style.width = ate.width;
                ate.style.height = ate.height;
                ate.hbForeGround.style.backgroundColor = ate.fillColor;
                ate.RegisterValueChangedCallback(ate.UpdateHealth);
                ate.FillHealth();
            }
        
        }

        public void UpdateHealth(ChangeEvent<float> evt)
        {
            FillHealth();
        }
        private void FillHealth()
        {
            if (fillType == FillType.Horizontal)
            {
                hbForeGround.style.scale = new Scale(new Vector3(value, 1, 0));

            }
            else
            {
                hbForeGround.style.scale = new Scale(new Vector3(1, value, 0));
            }
        }

    }
}
