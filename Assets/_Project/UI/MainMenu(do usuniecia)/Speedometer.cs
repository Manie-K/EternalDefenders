using System.Xml.XPath;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    [UxmlElement]
    public partial class Speedometer : VisualElement
    {
        [SerializeField, DontCreateProperty]
        float m_Progress;

        [UxmlAttribute, CreateProperty]
        public float progress
        {
            get => m_Progress;
            set
            {
                m_Progress = Mathf.Clamp(value, 0.01f, 100f);
                MarkDirtyRepaint();
            }
        }
        public Speedometer()
        {
            generateVisualContent += GenerateVisualContent;
        }

        private void GenerateVisualContent(MeshGenerationContext context)
        {
            float width = contentRect.width;
            float height = contentRect.height;

            var painter = context.painter2D;
            painter.BeginPath();
            painter.lineWidth = 10f;
            painter.Arc(new Vector2(width * 0.5f, height), width * 0.5f, 180f, 0f);
            painter.ClosePath();
            painter.fillColor = Color.white;
            painter.Fill(FillRule.NonZero);
            painter.Stroke();

            //FILL
            painter.BeginPath();
            painter.LineTo(new Vector2(width * 0.5f, height));
            painter.lineWidth = 10f;

            float amount = 180f * ((100f - progress) / 100f);

            painter.Arc(new Vector2(width * 0.5f, height), width * 0.5f, 180f, 0f - amount);
            painter.ClosePath();
            painter.fillColor = Color.green;
            painter.Fill(FillRule.NonZero);
            painter.Stroke();

            //ANOTHER FILL
            painter.BeginPath();
            painter.LineTo(new Vector2(0, height));
            painter.lineWidth = 10f;
            painter.Arc(new Vector2(width * 0.5f, height), width * 0.3f, 180f, 0f);
            painter.ClosePath();
            painter.fillColor = Color.black;
            painter.Fill(FillRule.NonZero);
            painter.Stroke();
        }
    }
}
